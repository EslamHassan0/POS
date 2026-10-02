using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using POS.Application.Contract.InvoiceItems;
using POS.Application.Contract.Invoices;
using POS.Application.Contract.Permissions;
using POS.Application.Contract.Users;
using POS.Application.Invoices.Events.Notifications;
using POS.Contract.IUnitOfWorks;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.OutboxMessages;
using POS.Domain.Entity.OutboxMessages.Events;
using POS.Domain.Entity.POSSettings;
using POS.Domain.Events;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.OutboxMessages;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;


namespace POS.Application.Invoices
{
    [Authorize(POSPermissions.Sales.Default)]
    public class InvoiceAppService : ApplicationService, IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IGenericRepository<POSSetting, Guid> _posSettingRepository;
        private readonly InvoiceManager _invoiceManager;
        private readonly OutboxMessageManager _outboxMessageManager;
        private readonly IMediator _mediator;
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        public InvoiceAppService(IInvoiceRepository invoiceRepository,
            InvoiceManager invoiceManager,
            IGenericRepository<POSSetting, Guid> posSettingRepository,
            IMediator mediator,
            IItemRepository itemRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            OutboxMessageManager outboxMessageManager, IMapper mapper) : base(mapper)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceManager = invoiceManager;
            _posSettingRepository = posSettingRepository;
            _mediator = mediator;
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _outboxMessageManager = outboxMessageManager;
        }
        [Authorize(POSPermissions.Sales.Create)]
        public async Task<Result<InvoiceDto>> CreateAsync(CreateInvoiceDto invoiceDto)
        {
            //1 Phase  Create invoice and invoice items 
            var settingPOS = _posSettingRepository.GetQueryable().FirstOrDefault();

            if (settingPOS is null)
                return Result<InvoiceDto>.Failure("POS settings not found");

            var defaultVAT = (settingPOS.DefaultVAT / 100);

            var itemIds = invoiceDto.InvoiceItems
                    .Select(a => a.ItemId)
                    .ToList();


            var itemsPrice = await _itemRepository.GetItemsByIds(itemIds);

            if (itemsPrice.Count != itemIds.Count)
                return Result<InvoiceDto>.Failure("One or more ItemIds are invalid");


            var itemsDictionary = itemsPrice.ToDictionary(x => x.Id);
            var invoiceItems = _mapper.Map<List<CreateInvoiceItemDto>, List<InvoiceItem>>(invoiceDto.InvoiceItems);

            foreach (var invoiceItem in invoiceItems)
            {
                var item = itemsDictionary[invoiceItem.ItemId];

                invoiceItem.Price = item.Price;
                invoiceItem.VatPercent = item.VatPercent > 0 ? item.VatPercent : defaultVAT;
                invoiceItem.TotalLine = invoiceItem.Price * invoiceItem.Qty;
            }

            var invoice = await _invoiceManager.CreateAsync(invoiceDto.InvoiceDate, invoiceDto.CustomerId,
                invoiceDto.PaymentType, invoiceDto.Discount, invoiceItems);

            if (!invoice.IsSuccess)
                return Result<InvoiceDto>.Failure(invoice.Error);


            //await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _invoiceRepository.AddAsync(invoice.Value);

                //await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
            return Result<InvoiceDto>.Success(_mapper.Map<Invoice, InvoiceDto>(invoice.Value));

        }

        public async Task<PagedResultDTo<InvoiceWithDetailsDto>> GetListAsync(InvoiceInput input)
        {
            var items = await _invoiceRepository.GetListAsync(input.FilterText, input.InvoiceNo, input.Sorting, input.MaxResultCount, input.SkipCount);
            var totalCount = await _invoiceRepository.GetCountAsync(input.FilterText, input.InvoiceNo, input.Sorting, input.MaxResultCount);

            var invoice = _mapper.Map<List<InvoiceWithDetails>, List<InvoiceWithDetailsDto>>(items);

            return new PagedResultDTo<InvoiceWithDetailsDto>(invoice, totalCount);
        }

        public async Task<InvoiceDto> GetAsync(Guid id)
        {
            return _mapper.Map<Invoice, InvoiceDto>(await _invoiceRepository.GetByIdAsync(id));
        }
      

        public async Task<InvoiceDto> GetWithDetailsAsync(Guid id)
        {
            return _mapper.Map<Invoice, InvoiceDto>(await _invoiceRepository.GetWithDetailsAsync(id));
        }
        public async Task<Result<InvoiceDto>> CancelAsync(Guid invoiceId)   
        {
            var userId = _currentUser.UserId;


            await _unitOfWork.BeginTransactionAsync();
            try
            {

                var invoice = await _invoiceManager.CancelAsync(invoiceId, userId.Value);

                if (!invoice.result.IsSuccess)
                    return Result<InvoiceDto>.Failure(invoice.result.Error);

                if (invoice.wasAlreadyPaid)
                {
                    await _mediator.Publish(new InvoiceCancelNotification(
                new InvoiceCancelEvent(invoice.result.Value.Id, invoice.result.Value.InvoiceItems.ToList())));
                }
                    await _unitOfWork.CommitAsync();
                return Result<InvoiceDto>.Success(_mapper.Map<Invoice, InvoiceDto>(invoice.result.Value));

            }
            catch (Exception ex)
            {

                await _unitOfWork.RollbackAsync();
                return Result<InvoiceDto>.Failure(ex.Message);
            }
        }
        public async Task<Result<InvoiceDto>> PayAsync(Guid invoiceId)
        {

            await _unitOfWork.BeginTransactionAsync();

            try
            {

                var userId = _currentUser.UserId;
                var invoice = await _invoiceManager.PayAsync(invoiceId, userId.Value);

                if (!invoice.IsSuccess)
                {
                    await _unitOfWork.RollbackAsync();

                    return Result<InvoiceDto>.Failure(invoice.Error);
                }
                await _mediator.Publish(new InvoicePaidNotification(
          new InvoicePaidEvent(invoice.Value.Id, invoice.Value.InvoiceItems.ToList())));

                await _outboxMessageManager.CreateMessageAsync(invoice.Value.Id,
                  OutboxMessageType.InvoicePaid,
                  new GenerateReceiptRequestedEvent
                  {
                      InvoiceId = invoice.Value.Id
                  });


                await _unitOfWork.CommitAsync();
                return Result<InvoiceDto>.Success(_mapper.Map<Invoice, InvoiceDto>(invoice.Value));
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return Result<InvoiceDto>.Failure("An error occurred while creating the invoice or updating inventory");
            }


            //return Result<InvoiceDto>.Success(_mapper.Map<Invoice, InvoiceDto>(invoice.Value));
        }

    }
}
