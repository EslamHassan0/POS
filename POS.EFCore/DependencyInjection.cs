using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Contract.IUnitOfWorks;
using POS.Domain.GenericRepository;
using POS.Domain.Identity;
using POS.Domain.Shared.Common;
using POS.EFCore.DatabaseSeeders;
using POS.EFCore.GenericRepository;
using POS.EFCore.GenericRepository.Common;
using POS.EFCore.GenericRepository.StockTransactions;
using POS.EFCore.POSContexts;
namespace POS.EFCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<POSContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityApplicationUser, IdentityRole<Guid>>(op =>
        {
            op.Lockout.AllowedForNewUsers = true;
            op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            op.Lockout.MaxFailedAccessAttempts = 5;
            op.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        })
        .AddEntityFrameworkStores<POSContext>()
        .AddDefaultTokenProviders();

        services.AddTransient<DatabaseSeeder>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        services.Scan(scan => scan
          .FromAssemblyOf<StockTransactionRepository>()
          .AddClasses(classes => classes.AssignableTo<IRepositoryMarker>())
          .AsImplementedInterfaces()
          .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, POS.EFCore.UnitOfWork.UnitOfWork>();

        services.AddScoped<IAsyncQueryableExecuter, EfAsyncQueryableExecuter>();

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfireServer();

        return services;

    }
}

