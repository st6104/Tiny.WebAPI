using System.ComponentModel.Design;
using System.Reflection;

namespace Tiny.Infrastructure.Extensions;

internal static class ModelBuilderExtension
{
    public static void ApplyEntityConfigurationsFromAssemblyContaining<T>(this ModelBuilder builder)
    {
        var configurationTypes = typeof(T).Assembly
                                            .GetTypes()
                                            .Where(type => !type.IsAbstract &&
                                                            !type.IsGenericTypeDefinition &&
                                                            type.GetTypeInfo()
                                                                .ImplementedInterfaces
                                                                .Any(i => i.GetTypeInfo().IsGenericType &&
                                                                        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                                            .ToList();

        configurationTypes.ForEach(type =>
        {
            dynamic config = Activator.CreateInstance(type)!;
            builder.ApplyConfiguration(config);
        });
    }
}
