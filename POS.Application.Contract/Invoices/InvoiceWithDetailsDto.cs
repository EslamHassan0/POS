using POS.Domain.Shared.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Invoices
{
    public class InvoiceWithDetailsDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }

        public decimal TotalBeforeTax { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Discount { get; set; }


        public decimal PaymentFee { get; set; }
        public decimal GrandTotal { get; set; }

        public InvoiceStatus InvoiceStatus { get; set; }
    }
}
