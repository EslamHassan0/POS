// POS.Application/PermissionHandler/RolePermissionSeeder.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using POS.Application.Contract.Permissions;
using System.Security.Claims;

namespace POS.Application.PermissionHandler;

public class RolePermissionSeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<RolePermissionSeeder> _logger;

    public RolePermissionSeeder(
        RoleManager<IdentityRole<Guid>> roleManager,
        ILogger<RolePermissionSeeder> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting roles & permissions seeding...");

        await CreateRoleIfNotExists("Admin");
        await CreateRoleIfNotExists("Cashier");
        await CreateRoleIfNotExists("Accountant");

        // Admin: كل الصلاحيات الموجودة في النظام
        await GrantPermissions("Admin", POSPermissions.GetAll());

        // Cashier: صلاحيات محددة بس
        await GrantPermissions("Cashier", new List<string>
        {
            POSPermissions.Sales.Create,
            POSPermissions.Sales.View,
            POSPermissions.Sales.Pay,
            POSPermissions.Sales.Print,
        });

        // Accountant: تقارير ومشتريات بس
        await GrantPermissions("Accountant", new List<string>
        {
            POSPermissions.Reports.ViewFinancial,
            POSPermissions.Reports.ViewSales,
            POSPermissions.Reports.ViewPurchases,
            POSPermissions.Purchases.View,
            POSPermissions.Purchases.Pay,
        });

        _logger.LogInformation("Roles & permissions seeding completed.");
    }

    private async Task CreateRoleIfNotExists(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to create role {RoleName}: {Errors}",
                    roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

    private async Task GrantPermissions(string roleName, List<string> permissions)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return;

        var existingClaims = await _roleManager.GetClaimsAsync(role);
        var existingPermissions = existingClaims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToHashSet();

        foreach (var permission in permissions)
        {
            if (existingPermissions.Contains(permission))
                continue; 

            var result = await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to grant {Permission} to {RoleName}: {Errors}",
                    permission, roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}