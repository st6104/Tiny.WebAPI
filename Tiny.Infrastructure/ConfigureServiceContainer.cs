using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Tiny.Infrastructure.Behaviors;
using Tiny.Shared.DomainService;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure;

public static class ConfigureServiceContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddDbContext<TinyContext>();

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
}

internal class MarkedAssemlbyClass { }
