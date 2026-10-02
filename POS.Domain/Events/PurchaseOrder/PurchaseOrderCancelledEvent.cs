using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Events.PurchaseOrder
{
    public class PurchaseOrderCancelledEvent
    {
        public Guid PurchaseId { get; }
        public List<PurchaseOrderItem> PurchaseOrderItems { get; }
        public PurchaseOrderCancelledEvent(Guid purchaseId, List<PurchaseOrderItem> purchaseOrderItems)
        {
            PurchaseId = purchaseId;
            PurchaseOrderItems = purchaseOrderItems;
        }
    }
}
