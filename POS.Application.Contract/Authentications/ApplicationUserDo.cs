using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Authentications
{
    public class ApplicationUserDo  
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        [Required, MaxLength(50)]
        public string FristName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }


        [Required, MaxLength(150)]
        public string Password { get; set; }
        [Required, MaxLength(250)]
        public string Email { get; set; }
        public DateTime LastLoginTime { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
