using Tiny.Infrastructure.Abstract;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.EntityConfigurations;

namespace Tiny.Infrastructure.Extensions;

internal static class ModelBuilderExtension
{
    public static void ApplyEntityConfigurationsFromAssemblyContaining<T>(this ModelBuilder builder, ITenantInfo currentTenant)
    {
        var configurationTypes = typeof(T).Assembly
                                            .GetTypes()
                                            .Where(type => type.IsClass &&
                                                            !type.IsAbstract &&
                                                            type.BaseType?.IsGenericType == true &&
                                                            type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfigurationBase<>))
                                            .ToList();

        configurationTypes.ForEach(type =>
        {
            dynamic config = Activator.CreateInstance(type, currentTenant)!;

            builder.ApplyConfiguration(config);
        });
    }
}
