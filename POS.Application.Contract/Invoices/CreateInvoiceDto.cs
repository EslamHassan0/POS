using POS.Application.Contract.InvoiceItems;
using POS.Domain.Shared.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Invoices
{
    public class CreateInvoiceDto
    {
        
        public DateTime InvoiceDate { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Discount { get; set; }
        //public decimal VATPercent { get; set; }
        public PaymentType PaymentType { get; set; }
        public List<CreateInvoiceItemDto> InvoiceItems { get; set; }

       
    }
}
