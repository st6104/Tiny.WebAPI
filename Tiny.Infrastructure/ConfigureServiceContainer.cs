using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Behaviors;
using Tiny.Infrastructure.MultiTenant;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure;

public static class ConfigureServiceContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddDbContext<TinyDbContext>();

        services.Scan(scan =>
        {
            scan.FromAssemblyOf<MarkedAssemlbyClass>()
                .WithRepositories();
        });

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<MarkedAssemlbyClass>();
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });

        services.AddScoped<IMultiTenantStore<TenantInfo>, MultiTenantStore>();
        services.AddScoped<IMultiTenantService, MultiTenantService>();

        return services;
    }
}

internal static class ScrutorExtension
{
    public static IImplementationTypeSelector WithRepositories(this IImplementationTypeSelector selector)
    {
        return selector.AddClasses(type => type.AssignableTo(typeof(IRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime();
    }

    //public static IImplementationTypeSelector WithEntityConfigurations(this IImplementationTypeSelector selector)
    //{
    //    return selector.AddClasses(type => type.AssignableTo(typeof(IEntityTypeConfiguration<>)))
    //                    .AsSelf()
    //                    .WithTransientLifetime();
    //}
}

internal class MarkedAssemlbyClass
{
}
