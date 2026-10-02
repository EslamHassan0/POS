using MediatR;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using POS.Domain.Events.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PurchaseOrders.Events.Notifications.PurchaseOrderCancelled
{
    public class PurchaseOrderCancelledNotification :INotification
    {
        public Guid PurchaseId { get; }
        public List<PurchaseOrderItem> Items { get; }

        public PurchaseOrderCancelledNotification(PurchaseOrderCancelledEvent domainEvent)
        {
            PurchaseId = domainEvent.PurchaseId;
            Items = domainEvent.PurchaseOrderItems;
        }
    }
}
