using POS.Application.Contract.InvoiceItems;
using POS.Domain.Shared.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Invoices
{
    public class InvoiceDto
    {
        public string? InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Guid CustomerId { get; set; }
        public string Customer { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Discount { get; set; }
        //public Payment Payment { get; set; }

        public decimal PaymentFee { get; set; }
        public decimal GrandTotal { get; set; }


        public string? ReceiptPath { get; set; }
        public DateTime? ReceiptGeneratedAt { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; } // = InvoiceStatus.Draft;
        public List<InvoiceItemDto> Items { get; set; }
    }
}
