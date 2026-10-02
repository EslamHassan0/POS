using Microsoft.AspNetCore.Http;
using POS.Application.Contract.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Users;

public class CurrentUser : ICurrentUser
{
    public Guid? UserId { get; }

    public string? Username { get; }

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;

        if (claimsPrincipal is null) return;

        Guid.TryParse(claimsPrincipal.FindFirstValue("uid"), out Guid userId);

        UserId = userId;
        Username = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
    }
}
