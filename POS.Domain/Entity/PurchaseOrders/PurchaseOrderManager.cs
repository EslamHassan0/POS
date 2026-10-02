using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using POS.Domain.Shared.PurchaseOrderStatus;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.PurchaseOrders
{
    public class PurchaseOrderManager :DomainServices
    {

        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderManager(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<Result<PurchaseOrder>> CreateAsync(
            DateTime date,
            Guid supplierId,
            decimal totalAmount,Guid warehouseId, List<PurchaseOrderItem> items)
        {
            var order = new PurchaseOrder(Guid.NewGuid(), " ", date, supplierId, Status.Pending,0, warehouseId);

            foreach (var item in items)
            {
                order.AddItem(item.ItemId,item.Qty,item.UnitPrice);
            }
           
            order.CalculateTotals();
            return Result<PurchaseOrder>.Success(order);

        }
        public async Task<Result<PurchaseOrder>> Receive(PurchaseOrder order)
        {
            if (order.Status != Status.Pending)
                return Result<PurchaseOrder>.Failure("Order already received or cancelled");

            var orderNo = await _purchaseOrderRepository.GetNextOrderNo();
            order.PurchaseOrderNo = orderNo;
            order.Receive();
            //order.AddDomainEvent(new PurchaseOrderReceivedEvent(order.Id, order.Items));

            return Result<PurchaseOrder>.Success(order);
        }

        public Result<PurchaseOrder> Cancel(PurchaseOrder order)
        {
            if (order.Status != Status.Pending)
                return Result<PurchaseOrder>.Failure("Order already received or cancelled");

            order.Cancel();            
            return Result<PurchaseOrder>.Success(order);
        }

        public async Task<Result<PurchaseOrder>> AttachReceiptAsync(Guid id, string receiptPath)
        {
            var invoice = await _purchaseOrderRepository.GetByIdAsync(id);

            invoice.ReceiptPath = receiptPath;
            invoice.ReceiptGeneratedAt = DateTime.UtcNow;
            return Result<PurchaseOrder>.Success(await _purchaseOrderRepository.UpdateAsync(invoice));
        }
    }
}
