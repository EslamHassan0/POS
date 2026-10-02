using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.BackgroundJobs;
using POS.Application.Contract;
using POS.Application.Contract.FileStorages;
using POS.Application.Contract.Users;
using POS.Application.PermissionHandler;
using POS.Application.Users;
using POS.Domain.Shared.Helpers;

namespace POS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.Configure<OptionsUserRole>(configuration.GetSection("OptionsUserRole"));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationService).Assembly);
        });

        services.AddAutoMapper(typeof(ApplicationService).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<ApplicationService>()
            .AddClasses(classes => classes.AssignableTo<IApplicationService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());


        services.AddScoped<RolePermissionSeeder>();

        services.Configure<FileUploadOptions>(configuration.GetSection("FileUpload"));
        services.Configure<FileStorage>(configuration.GetSection("FileStorage"));


        services.Configure<JWT>(configuration.GetSection("JWT"));

        //Permission and Policy
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, POS.Application.PermissionHandler.PermissionHandler>();
        services.AddScoped<IClaimsTransformation, PermissionClaimsTransformation>();

        services.AddHostedService<OutboxDispatcher>();

        return services;
    }
}
