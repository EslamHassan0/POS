using POS.Domain.Entity.InvoiceItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Events
{
    public class InvoiceCancelEvent
    {
        public Guid InvoiceId { get; }
        public List<InvoiceItem> InvoiceItems { get; }


        public InvoiceCancelEvent(Guid invoiceId, List<InvoiceItem> invoiceItems)
        {
            InvoiceId = invoiceId;
            InvoiceItems = invoiceItems;
        }
    }
}
