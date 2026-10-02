using MediatR;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using POS.Domain.Events;
using POS.Domain.Events.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PurchaseOrders.Events.Notifications.PurchaseOrderReceived
{
    public class PurchaseOrderReceivedNotification : INotification
    {
        public Guid PurchaseId { get; }
        public List<PurchaseOrderItem> Items { get; }

        public PurchaseOrderReceivedNotification(PurchaseOrderReceivedEvent domainEvent)
        {
            PurchaseId = domainEvent.PurchaseId;
            Items = domainEvent.PurchaseOrderItems;
        }
    }
}
