using AutoMapper;
using MediatR;
using POS.Application.Contract.Invoices;
using POS.Application.Contract.Items;
using POS.Application.Contract.PurchaseOrders;
using POS.Application.Invoices.Events.Notifications;
using POS.Application.PurchaseOrders.Events.Notifications;
using POS.Application.PurchaseOrders.Events.Notifications.PurchaseOrderCancelled;
using POS.Application.PurchaseOrders.Events.Notifications.PurchaseOrderReceived;
using POS.Contract.IUnitOfWorks;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.OutboxMessages;
using POS.Domain.Entity.OutboxMessages.Events;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using POS.Domain.Entity.Suppliers;
using POS.Domain.Events;
using POS.Domain.Events.PurchaseOrder;
using POS.Domain.Shared.OutboxMessages;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PurchaseOrders
{
    public class PurchaseOrderAppService : ApplicationService, IPurchaseOrderAppService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly PurchaseOrderManager _purchaseOrderManager;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly OutboxMessageManager _outboxMessageManager;
        public PurchaseOrderAppService(ISupplierRepository supplierRepository,
            IItemRepository itemRepository,

            IPurchaseOrderRepository purchaseOrderRepository,
            PurchaseOrderManager purchaseOrderManager,
            IMapper mapper,
            IMediator mediator,
            IUnitOfWork unitOfWork,
            OutboxMessageManager outboxMessageManager) : base(mapper)
        {
            _supplierRepository = supplierRepository;
            _itemRepository = itemRepository;

            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderManager = purchaseOrderManager;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _outboxMessageManager = outboxMessageManager;
        }

        public async Task<PagedResultDTo<PurchaseOrderDto>> GetListAsync(PurchaseOrderInput input)
        {
            var items = await _purchaseOrderRepository.GetListAsync(input.OrderNo, input.Sorting, input.MaxResultCount);
            var totalCount = await _purchaseOrderRepository.GetCountAsync(input.OrderNo);

            
            var order = _mapper.Map<List<PurchaseOrderWithDetails>, List<PurchaseOrderDto>>(items);

            return new PagedResultDTo<PurchaseOrderDto>(order, totalCount);
        }
        public async Task<PurchaseOrderWithDetailsDto> GetAsync(Guid id)
        {
            var items = await _purchaseOrderRepository.GetAsync(id);

            var order = _mapper.Map<PurchaseOrderWithDetails, PurchaseOrderWithDetailsDto>(items);

            return order;
        }
        public async Task<Result<PurchaseOrderDto>> CreateAsync(CreatePurchaseOrderDto dto)
        {
           var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId);
            if (supplier is null)
                return Result<PurchaseOrderDto>.Failure("Supplier not found");
            var itemIds = dto.Items.Select(a => a.ItemId).ToList();
            var items = await _itemRepository.GetItemsByIds(itemIds);
            if (items.Count != itemIds.Count)
                return Result<PurchaseOrderDto>.Failure("One or more ItemIds are invalid");

            var purchaseOrderItems = dto.Items.Select(x => new PurchaseOrderItem(Guid.NewGuid(),Guid.Empty,x.ItemId,x.Qty,x.UnitPrice)
            {
                Id = Guid.NewGuid(),
                ItemId = x.ItemId,
                Qty = x.Qty,
                UnitPrice = x.UnitPrice,
                TotalLine = x.Qty * x.UnitPrice
            }).ToList();

            var order = await _purchaseOrderManager.CreateAsync(
                   dto.Date, dto.SupplierId,0, dto.WarehouseId, purchaseOrderItems
               );

            //foreach (var item in dto.Items)
            //{
            //    var orderItem = new PurchaseOrderItem
            //    {
            //        ItemId = item.ItemId,
            //        Qty = item.Qty,
            //        UnitCost = item.UnitCost,
            //        TotalLine = item.Qty * item.UnitCost,
            //        PurchaseOrderId = order.IsSuccess == true ? order.Value.Id : Guid.Empty

            //    };
            //    order.Value?.AddItem(orderItem);
            //}


            if (!order.IsSuccess)
                return Result<PurchaseOrderDto>.Failure(order.Error);

            await _purchaseOrderRepository.AddAsync(order.Value);
            return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrder, PurchaseOrderDto>(order.Value));
        }


        public async Task<Result<PurchaseOrderDto>> ReceiveAsync(Guid orderId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = await _purchaseOrderRepository.GetPurchaseOrderWithDetailsById(orderId);
                if (order is null)
                    return Result<PurchaseOrderDto>.Failure("Order not found");

                var result = await _purchaseOrderManager.Receive(order);
                if (!result.IsSuccess)
                    return Result<PurchaseOrderDto>.Failure(result.Error);


                if (result.IsSuccess)
                {
                    await _mediator.Publish(new PurchaseOrderReceivedNotification(

                new PurchaseOrderReceivedEvent(result.Value.Id, result.Value.PurchaseOrderItems.ToList())));

                    await _outboxMessageManager.CreateMessageAsync(result.Value.Id,
                        OutboxMessageType.PurchaseOrderReceived,
                        new GeneratePurchaseReceiptRequestedEvent
                        {
                            PurchaseOrderId = result.Value.Id
                        });
                }

                await _purchaseOrderRepository.UpdateAsync(order);
                return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(order));
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public async Task<Result<PurchaseOrderDto>> CancelAsync(Guid orderId)
        {
            var order = await _purchaseOrderRepository.GetPurchaseOrderWithDetailsById(orderId);
            if (order is null)
                return Result<PurchaseOrderDto>.Failure("Order not found");

            var result = _purchaseOrderManager.Cancel(order);
            if (!result.IsSuccess)
                return Result<PurchaseOrderDto>.Failure(result.Error);


            if (result.IsSuccess)
            {
                await _mediator.Publish(new PurchaseOrderCancelledNotification(

            new PurchaseOrderCancelledEvent(result.Value.Id, result.Value.PurchaseOrderItems.ToList())));

            }

            await _purchaseOrderRepository.UpdateAsync(order);
            return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(order));
        }
    }
}
