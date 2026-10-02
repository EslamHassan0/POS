
using System.ComponentModel.DataAnnotations;

namespace POS.Domain.Entity.Users;

public class ApplicationUser  
{
    public Guid Id { get; set; }
    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }


    [Required, MaxLength(150)]
    public string Password { get; set; }
    [Required, MaxLength(250)]
    public string Email { get; set; }
    public DateTime LastLoginTime { get; set; }
    //public List<RefreshToken>? RefreshTokens { get; set; }
}
