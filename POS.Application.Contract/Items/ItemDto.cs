using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Items
{
    public class ItemDto
    {
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
        public decimal StockQty { get; set; }
        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }


        [MaxLength(50)]
        public string? ImageUrl { get; set; }
    }
}
