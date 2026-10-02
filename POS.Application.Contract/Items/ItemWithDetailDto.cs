using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Shared.Item;

namespace POS.Application.Contract.Items
{
    public class ItemWithDetailDto
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal VatPercent { get; set; }
        public bool IsVATEligible { get; set; }
        public bool IsActive { get; set; }
        public decimal StockQty { get; set; }
        public string CategoryName { get; set; }

        public ItemUnit Unit { get; set; }
        //[Required]
        [MaxLength(50)]
        public string? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
