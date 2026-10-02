using POS.Domain.BaseEntity;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.Payments;
using POS.Domain.Shared.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Invoices
{
    public class Invoice : BaseEntity<Guid>
    {

        public string? InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Discount { get; set; }  
        public Payment Payment { get; set; }

        public decimal PaymentFee { get; set; }  
        public decimal GrandTotal { get; set; }


        public string? ReceiptPath { get; set; }
        public DateTime? ReceiptGeneratedAt { get; set; }
        public InvoiceStatus  InvoiceStatus { get; set; } // = InvoiceStatus.Draft;

        public  ICollection<InvoiceItem> InvoiceItems { get; private set; }
        public Invoice(Guid id, string? invoiceNo, DateTime invoiceDate, Guid customerId,
                        PaymentType paymentType, decimal discount)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer is required");
            Id = id;
            InvoiceNo = invoiceNo;
            InvoiceDate = invoiceDate;
            CustomerId = customerId;
            PaymentType = paymentType;
            Discount = discount;
            InvoiceItems = new List<InvoiceItem>();
           
        }

        public void CalculateTotals()
        {
            TotalBeforeTax = InvoiceItems.Sum(x => x.TotalLine);
            VatAmount = InvoiceItems.Sum(x => x.VatValue);
            TotalAfterTax = TotalBeforeTax + VatAmount;

            TotalAfterDiscount = TotalAfterTax - Discount;

            PaymentFee = PaymentType == PaymentType.Visa
                ? TotalAfterDiscount * 0.02m
                : 0;

            GrandTotal = TotalAfterDiscount + PaymentFee;
        }

        public void AddInvoiceItem(InvoiceItem invoiceItem)
        {
            InvoiceItems.Add(new InvoiceItem(Guid.NewGuid(), this.Id, invoiceItem.ItemId, invoiceItem.Qty, invoiceItem.Price, invoiceItem.TotalLine, invoiceItem.VatPercent, invoiceItem.VatValue, invoiceItem.TotalWithVat));
        }
    }
}
