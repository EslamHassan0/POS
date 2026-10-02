using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Warehouses
{
    public class UpdateWarehouseDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Location { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
