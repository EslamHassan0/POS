using POS.Domain.BaseEntity;
using POS.Domain.Entity.Categories;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Shared.Common;
using POS.Domain.Shared.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Items
{
    public class Item : BaseEntity<Guid> , IConcurrentEntity
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public string Barcode { get; set; }
        
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal VatPercent { get; set; }
        public bool IsVATEligible { get; set; }
        public bool IsActive { get; set; }

        public ItemUnit Unit { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }



        public string? Image { get; set; }   

        public ICollection<InvoiceItem> InvoiceItems { get; }

        //[Timestamp]
        public byte[] RowVersion { get; set; } // Concurrency token

        public Item(Guid id, string name, string code, string barcode, decimal price, decimal cost,
                    decimal vatPercent, bool isVATEligible, bool isActive, Guid categoryId, string? image, ItemUnit unit)
        {
            Id = id;
            Name = name;
            Code = code;
            Barcode = barcode;
            Price = price;
            Cost = cost;
            VatPercent = vatPercent;
            IsVATEligible = isVATEligible;
            IsActive = isActive;
            CategoryId = categoryId;

            Image = image;
            Unit = unit;
            InvoiceItems = new List<InvoiceItem>();
        }
    }
}
