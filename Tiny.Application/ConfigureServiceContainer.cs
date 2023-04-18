using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Tiny.Application.Bahaviors;
using Tiny.Shared.DomainService;

namespace Tiny.Application;

public static class ConfigureServiceContainer
{
    public static IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssemblyContaining<MarkedAssemlbyClass>();
            configure.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.Scan(scan => scan.FromAssemblyOf<MarkedAssemlbyClass>()
                                    .WithDomainServices());

        services.AddValidatorsFromAssemblyContaining<MarkedAssemlbyClass>();

        services.AddMemoryCache();

        return services;
    }
}

internal static class ScrutorExtension
{
    public static IImplementationTypeSelector WithDomainServices(this IImplementationTypeSelector selector)
    {
        return selector.AddClasses(type => type.AssignableTo<IDomainService>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime();
    }
}

internal class MarkedAssemlbyClass { }
