
using Microsoft.Extensions.DependencyInjection;

namespace POS.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {

        services.Scan(scan => scan
                   .FromAssemblyOf<DomainServices>()
                   .AddClasses(classes => classes.AssignableTo<DomainServices>())
                   .AsSelf()
                   .WithTransientLifetime());

        return services;
    }
}
