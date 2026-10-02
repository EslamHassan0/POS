using POS.Domain.BaseEntity;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using POS.Domain.Entity.Suppliers;
using POS.Domain.Shared.PurchaseOrderStatus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.PurchaseOrders
{
    public class PurchaseOrder : BaseEntity<Guid>
    {
        public PurchaseOrder(Guid id, string? purchaseOrderNo,
            DateTime date,
            Guid supplierId,
            Status status,
            decimal totalAmount, Guid warehouseId)
        {
            Id = id;
            PurchaseOrderNo = purchaseOrderNo;
            Date = date;
            SupplierId = supplierId;
            Status = status;
            TotalAmount = totalAmount;
            PurchaseOrderItems = new Collection<PurchaseOrderItem>();
            WarehouseId = warehouseId;
        }

        public string? PurchaseOrderNo { get; set; }
        public DateTime Date { get; set; }
        public Guid SupplierId { get; set; }
        public Guid WarehouseId { get; set; }  
        public Supplier Supplier { get; set; }
        public Status Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ReceiptGeneratedAt { get; set; }
        public string? ReceiptPath { get; set; }
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; protected set; }


        public void AddItem(Guid itemId, decimal qty, decimal unitPrice)
        {
            PurchaseOrderItems.Add(new PurchaseOrderItem(Guid.NewGuid(), this.Id, itemId, qty, unitPrice));
        }

        public void CalculateTotals()
        {
            TotalAmount = PurchaseOrderItems.Sum(x => x.Qty * x.UnitPrice);
        }

        
        public void Receive()
        {
            if (Status != Status.Pending)
                throw new Exception("Order already received or cancelled");

            Status = Status.Received;
        }
        public void Cancel()
        {
            if (Status == Status.Cancelled)
                throw new Exception("Order already cancelled");


            Status = Status.Cancelled;
        }
    }
}
