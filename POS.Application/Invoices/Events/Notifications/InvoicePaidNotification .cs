using MediatR;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Invoices.Events.Notifications
{
    public class InvoicePaidNotification : INotification
    {
        public Guid InvoiceId { get; }
        public List<InvoiceItem> Items { get; }

        public InvoicePaidNotification(InvoicePaidEvent domainEvent)
        {
            InvoiceId = domainEvent.InvoiceId;
            Items = domainEvent.InvoiceItems;
        }
    }
}
