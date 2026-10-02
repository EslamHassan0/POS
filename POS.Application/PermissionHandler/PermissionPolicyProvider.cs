using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using POS.Application.Contract.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PermissionHandler;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{

    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    private readonly HashSet<string> _allPermissions;
    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (_allPermissions.Contains(policyName))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return Task.FromResult(policy.Build());
        }

        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }

 

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);

        _allPermissions = typeof(POSPermissions)
            .GetNestedTypes(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Append(typeof(POSPermissions))
            .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .Select(f => f.GetValue(null)?.ToString())
            .ToHashSet();
    }
    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return await FallbackPolicyProvider.GetDefaultPolicyAsync();
    }

     
    public async Task<AuthorizationPolicy> GetFallbackPolicyAsync()
    {
        return await FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}