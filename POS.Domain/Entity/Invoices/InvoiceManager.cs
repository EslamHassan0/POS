using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.StockTransactions;
using POS.Domain.Events;
using POS.Domain.Shared.Invoices;
using POS.Domain.Shared.Results;


namespace POS.Domain.Entity.Invoices
{
    public class InvoiceManager : DomainServices
    {
        private readonly  IInvoiceRepository _invoiceRepository;
        private readonly IStockTransactionRepository _stockTransactionRepository;
        private readonly IItemRepository _itemRepository;
        public InvoiceManager(IInvoiceRepository invoiceRepository,
            IStockTransactionRepository stockTransactionRepository, 
            IItemRepository itemRepository)
        {
            _invoiceRepository = invoiceRepository;
            _stockTransactionRepository = stockTransactionRepository;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Invoice>> CreateAsync(DateTime invoiceDate, Guid customerId,
                       PaymentType paymentType,decimal discount, List<InvoiceItem> InvoiceItems)
        {
          
            var invoice = new Invoice(Guid.NewGuid(), null , invoiceDate , customerId , paymentType, discount);


            var itemIds = InvoiceItems.Select(a => a.ItemId)
                                      .ToList();

            var itemsDictionary = _itemRepository.GetQueryable()
                .Where(a => itemIds.Contains(a.Id))
                .ToDictionary(x=>x.Id,n=>n.Name);

            var stockDictionary = await _stockTransactionRepository.GetQueryable()
                .Where(a => itemIds.Contains(a.ItemId))
                .GroupBy(a => a.ItemId)
                .Select(g => new { ItemId = g.Key, Available = g.Sum(x => x.QtyChange) })
                .ToDictionaryAsync(a => a.ItemId, x => x.Available);

            foreach (var item in InvoiceItems)
            {
                var itemName = itemsDictionary.GetValueOrDefault(item.ItemId, "Unknown");
                var currentStock = stockDictionary.GetValueOrDefault(item.ItemId,0);

                if(item.Qty > currentStock)
                    return Result<Invoice>.Failure($"Not enough stock for item {itemName}");

                item.CalculateTotals();
                invoice.AddInvoiceItem(item);
            }

            invoice.CalculateTotals();

            return Result<Invoice>.Success(invoice);
        }

        public async Task<(Result<Invoice> result ,bool wasAlreadyPaid)> CancelAsync(Guid invoiceId, Guid userId)
        {
            var invoice = await _invoiceRepository.GetWithDetailsAsync(invoiceId);

            if (invoice.InvoiceStatus == InvoiceStatus.Cancelled)
                return (Result<Invoice>.Failure("Cannot cancel a this invoice. already Returned "), false);

            var wasAlreadyPaid = invoice.InvoiceStatus == InvoiceStatus.Paid;

            invoice.InvoiceStatus = InvoiceStatus.Cancelled;
            invoice.ModifiedDate = DateTime.UtcNow;
            invoice.ModifiedBy = userId;
           await _invoiceRepository.UpdateAsync(invoice);

            return (Result<Invoice>.Success(invoice), wasAlreadyPaid);
        }

        public async Task<Result<Invoice>> PayAsync(Guid invoiceId, Guid userId)
        {
            var invoiceNo = await _invoiceRepository.GetNextInvoice();
            var invoice = await _invoiceRepository.GetWithDetailsAsync(invoiceId);

            if (invoice.InvoiceStatus != InvoiceStatus.Draft)
                return Result<Invoice>.Failure("This invoice cannot be paid (already paid or cancelled)");

            if (invoice.InvoiceStatus == InvoiceStatus.Paid)
                return Result<Invoice>.Failure("Cannot  a paid invoice. already paid.");

            if (invoice.InvoiceStatus == InvoiceStatus.Cancelled)  
                return Result<Invoice>.Failure("cannot pay this invoice because it has already been returned.  ");

            var itemIds = invoice.InvoiceItems.Select(x => x.ItemId).ToList();

            var stockDictionary = await _stockTransactionRepository.GetQueryable()
                .Where(x => itemIds.Contains(x.ItemId))
                .GroupBy(x => x.ItemId)
                .Select(g => new { ItemId = g.Key, Available = g.Sum(x => x.QtyChange) })
                .ToDictionaryAsync(x => x.ItemId, x => x.Available);

            var itemsDictionary = await _itemRepository.GetQueryable()
                .Where(x => itemIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x.Name);

            foreach (var item in invoice.InvoiceItems)
            {

                var itemName = itemsDictionary.GetValueOrDefault(item.ItemId, "Unknown");
                var currentStock = stockDictionary.GetValueOrDefault(item.ItemId, 0);

                if (item.Qty > currentStock)
                    return Result<Invoice>.Failure($"Not enough stock for item {itemName}");
            }

            invoice.InvoiceStatus = InvoiceStatus.Paid;
            invoice.ModifiedDate = DateTime.UtcNow;
            invoice.ModifiedBy = userId;
            invoice.InvoiceNo = invoiceNo;
            await _invoiceRepository.UpdateAsync(invoice);

            return Result<Invoice>.Success(invoice);
             
        }


        public async Task<Result<Invoice>> AttachReceiptAsync(Guid id,string receiptPath)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);

            invoice.ReceiptPath = receiptPath;
            invoice.ReceiptGeneratedAt = DateTime.UtcNow;
            return Result<Invoice>.Success(await _invoiceRepository.UpdateAsync(invoice));
        }
    }
}
