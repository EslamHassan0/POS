using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.Permissions;

namespace POS.API.Controllers.Permissions;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionAppService _permissionService;

    public PermissionsController(IPermissionAppService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet("GetSystemPermissions")]
    public async Task<IActionResult> GetSystemPermissions() 
    {
        var result = await _permissionService.GetSystemPermissionsAsync();
        return Ok(result);
    }

    [HttpGet("GetCurrentUserPermissions/{userId}")]
    public async Task<IActionResult> GetCurrentUserPermissionsAsync(Guid userId)
    {
        var result = await _permissionService.GetCurrentUserPermissionsAsync(userId);

        return Ok(result);
    }

    [HttpGet("GetRolePermissions/{roleId}")]
    public async Task<IActionResult> GetRolePermissionsAsync(string roleId)
    {
        var result = await _permissionService.GetRolePermissionsAsync(roleId);
        return Ok(result);
    }

    [HttpPut("UpdateRolePermissions/{roleId}")]
    public async Task<IActionResult> UpdateRolePermissions(string roleId, [FromBody] List<string> permissions)
    {
        var success = await _permissionService.UpdateRolePermissionsAsync(roleId, permissions);

        if (!success)
            return BadRequest("An error occurred while updating permissions.");

        return Ok("update was successfully completed"); 
    }

    [HttpPut("AssignPermissionToUser/{userId}")]
    public async Task<IActionResult> AssignPermissionToUserAsync(Guid userId, string permission)
    {
        var success = await _permissionService.AssignPermissionToUserAsync(userId, permission);

        if (!success)
            return BadRequest("An error occurred while updating permissions.");

        return Ok("Assign was successfully completed");
    }
}
