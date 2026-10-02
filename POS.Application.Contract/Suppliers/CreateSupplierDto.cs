using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Suppliers
{
     public class CreateSupplierDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
