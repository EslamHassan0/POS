using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using POS.Domain.Shared.Item;

namespace POS.Application.Contract.Items
{
    public class CreateItemDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal VatPercent { get; set; }
        public bool IsVATEligible { get; set; }
  
        public Guid CategoryId { get; set; }
        public ItemUnit Unit { get; set; }
        public IFormFile? Image { get; set; }
    }
}
