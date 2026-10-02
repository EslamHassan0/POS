using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Permissions;

public interface IPermissionAppService : IApplicationService
{
    Task<bool> AddPermissionToRole(string roleName, string permission);

    Task<List<string>> GetSystemPermissionsAsync();

    Task<List<string>> GetCurrentUserPermissionsAsync(Guid userId);

    Task<bool> AssignPermissionToUserAsync(Guid userId, string permission);
    Task<List<string>> GetRolePermissionsAsync(string roleId);

    Task<bool> UpdateRolePermissionsAsync(string roleId, List<string> newPermissions);
}
