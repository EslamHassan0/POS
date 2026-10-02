using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using POS.Domain.Entity.Users;

namespace POS.Domain.Identity
{
    public class IdentityApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginTime { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
