using POS.Application.Contract.InvoiceItems;
using POS.Domain.Shared.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Invoices
{
    public class UpdateInvoiceDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Guid CustomerId { get; set; }

        public decimal TotalBeforeTax { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public PaymentType PaymentType { get; set; }
        public ICollection<UpdateInvoiceItemDto> InvoiceItems { get; }
    }
}
