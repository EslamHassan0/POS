using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PermissionHandler;

public class PermissionClaimsTransformation : IClaimsTransformation
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public PermissionClaimsTransformation(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
            return principal;

        var identity = (ClaimsIdentity)principal.Identity;

        //no more request 
        if (identity.HasClaim(c => c.Type == "permission"))
            return principal;

        var roleNames = principal.FindAll(ClaimTypes.Role).Select(c => c.Value);
        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims.Where(c => c.Type == "permission"))
            {
                identity.AddClaim(claim);
            }
        }

        return principal;

    }
}
