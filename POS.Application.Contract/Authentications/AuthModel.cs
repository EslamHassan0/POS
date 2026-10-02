using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace POS.Application.Contract.Authentications;

public class AuthModel
{
    public string Massage { get; set; }
    public bool IsAuthenticated { get; set; }
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public List<String> Roles { get; set; }
    public string Token { get; set; }
    ///   public DateTime? ExpiresOn { get; set; }
    public DateTime LastLoginTime { get; set; }


    [JsonIgnore]
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }

}

