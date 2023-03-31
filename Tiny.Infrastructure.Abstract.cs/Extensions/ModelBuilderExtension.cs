using Tiny.Infrastructure.Abstract.EntityTypeConfigure;

namespace Tiny.Infrastructure.Abstract.Extensions;

internal static class ModelBuilderExtension
{
    internal static void ApplyEntityConfigurationsFromAssemblyContaining<T>(this ModelBuilder builder)
    {
        builder.ApplyEntityConfigurationsFromAssemblyContaining(typeof(T));
    }
    
    internal static void ApplyEntityConfigurationsFromAssemblyContaining(this ModelBuilder builder, Type type)
    {
        var configurationTypes = type.Assembly
            .GetTypes()
            .Where(classType => classType.IsClass &&
                                !classType.IsAbstract &&
                                classType.BaseType?.IsGenericType == true &&
                                classType.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfigurationBase<>))
            .ToList();

        configurationTypes.ForEach(configType =>
        {
            dynamic config = Activator.CreateInstance(configType)!;
            builder.ApplyConfiguration(config);
        });
    }
}
