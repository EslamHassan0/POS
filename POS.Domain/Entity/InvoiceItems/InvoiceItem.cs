using POS.Domain.BaseEntity;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.InvoiceItems
{
    public class InvoiceItem : BaseEntity<Guid>
    {

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }

        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalLine { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatValue { get; set; }
        public decimal TotalWithVat { get; set; }


        public InvoiceItem() { }



        public void CalculateTotals()
        {
            if(Qty <=0)
                throw new Exception("Quantity must be greater than zero");

            if(Price <0)
                throw new Exception("Price cannot be negative");

            TotalLine = Qty * Price;
            VatValue = TotalLine * VatPercent;
            TotalWithVat = TotalLine + VatValue;

        }
        public InvoiceItem(Guid id, Guid invoiceId, Guid itemId, decimal qty, decimal price,
                            decimal totalLine, decimal vatPercent, decimal vatValue, decimal totalWithVat)
        {
            Id = id;
            InvoiceId = invoiceId;
            ItemId = itemId;
            Qty = qty;
            Price = price;
            TotalLine = totalLine;
            VatPercent = vatPercent;
            VatValue = vatValue;
            TotalWithVat = totalWithVat;
           
        }
    }
}
