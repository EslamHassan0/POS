using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Users
{
    public class CreateUpdateUserDto
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength (50)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
