using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tiny.Application.Bahaviors;

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

        services.AddValidatorsFromAssemblyContaining<MarkedAssemlbyClass>();

        return services;
    }
}

internal class MarkedAssemlbyClass { }
