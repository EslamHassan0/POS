using POS.Domain.BaseEntity;
using POS.Domain.Entity.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems
{
    public class PurchaseOrderItem : BaseEntity<Guid>
    {
        public PurchaseOrderItem(Guid id , Guid purchaseOrderId, Guid itemId, decimal qty, decimal unitPrice)
        {
            Id = id;
            Qty = qty;
            UnitPrice = unitPrice;
            //TotalLine = totalLine;
            PurchaseOrderId = purchaseOrderId;
            ItemId = itemId;
            CalculateTotalLine();
        }

        protected PurchaseOrderItem()
        {
            
        }

        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalLine { get; set; }

        public Guid PurchaseOrderId { get; set; }
        //public PurchaseOrder PurchaseOrder { get; set; }

        // FK to Item
        public Guid ItemId { get; set; }
        public Item Item { get; set; }

        private void CalculateTotalLine()
        {

            if (Qty <= 0)
                throw new ArgumentException("Qty must be greater than 0");

            if (UnitPrice < 0)
                throw new ArgumentException("UnitPrice cannot be negative");

            TotalLine = Qty * UnitPrice;
        }
    }
}
