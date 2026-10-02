using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using POS.Application.Contract.Permissions;
using POS.Domain.Entity.Users;
using POS.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Permissions;

public class PermissionAppService : ApplicationService, IPermissionAppService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<IdentityApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public PermissionAppService(RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<IdentityApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,IMapper mapper) : base(mapper)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> AddPermissionToRole(string roleName, string permission)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return false;

       
        var claims = await _roleManager.GetClaimsAsync(role);
        if (claims.Any(c => c.Type == "permission" && c.Value == permission))
            return true;

        var result = await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
        return result.Succeeded;
    }
    public async Task<bool> AssignPermissionToUserAsync(Guid userId, string permission)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

         
        var claim = new Claim("permission", permission);
        var result = await _userManager.AddClaimAsync(user, claim);

        return result.Succeeded;
    }

    public async Task<List<string>> GetSystemPermissionsAsync()
    {
        var permissions = new List<string>();
        var classes = typeof(POSPermissions).GetNestedTypes();
        foreach (var cls in classes)
        {
            var fields = cls.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                permissions.Add(field.GetValue(null).ToString());
            }
        }
        return permissions;
    }

    public async Task<List<string>> GetCurrentUserPermissionsAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) 
            return new List<string>();

        var claims = await _userManager.GetClaimsAsync(user);
        return claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();
    }
    public async Task<List<string>> GetRolePermissionsAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return new List<string>();

        var claims = await _roleManager.GetClaimsAsync(role);
        return claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();
    }
    public async Task<bool> UpdateRolePermissionsAsync(string userId, List<string> newPermissions)
    {
        var user = await _userManager.FindByIdAsync(userId);
        //var role = await _roleManager.FindByIdAsync(roleId);
        if (user == null) return false;



        var currentClaims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in currentClaims.Where(c => c.Type == "permission"))
        {
            await _userManager.RemoveClaimAsync(user, claim);
        }

        
        foreach (var permission in newPermissions)
        {
            await _userManager.AddClaimAsync(user, new Claim("permission", permission));
        }

        return true;
    }
}
