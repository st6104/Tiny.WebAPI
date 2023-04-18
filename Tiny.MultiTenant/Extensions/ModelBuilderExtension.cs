using Microsoft.EntityFrameworkCore;
using Tiny.MultiTenant.Abstract.Interfaces;
using Tiny.MultiTenant.EntityTypeConfigure;
using Tiny.MultiTenant.Services;
using Tiny.QueryFilterBuilder;
using Tiny.Shared.DomainEntity;
using Tiny.Shared.Extensions;

namespace Tiny.MultiTenant.Extensions;

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

    internal static void AddTenantIdProperty(this ModelBuilder builder, string fieldName)
    {
        var entityTypeBuilders = builder.Model.GetEntityTypes()
            .Where(mutableType => mutableType.ClrType.IsImplemented<IHasTenantId>() &&
                                  !mutableType.IsExistProperty(fieldName))
            .Select(mutableType => builder.Entity(mutableType.ClrType));

        foreach (var entityTypeBuilder in entityTypeBuilders)
        {
            entityTypeBuilder.AddTenantIdProperty(fieldName);
        }
    }

    internal static void AddDeletedAtProperty(this ModelBuilder builder, string fieldName)
    {
        var entityTypeBuilders = builder.Model.GetEntityTypes()
            .Where(mutableType => mutableType.ClrType.IsImplemented<ISoftDeletable>() &&
                                  !mutableType.IsExistProperty(fieldName))
            .Select(mutableType => builder.Entity(mutableType.ClrType));

        foreach (var entityTypeBuilder in entityTypeBuilders)
        {
            entityTypeBuilder.AddDeletedAtProperty(fieldName);
        }
    }

    internal static void AddQueryFilter(this ModelBuilder builder, string tenantId)
    {
        var entityTypeBuilders = builder.Model.GetEntityTypes()
            .Where(mutableType => mutableType.ClrType.IsImplemented<IHasTenantId>() ||
                                    mutableType.ClrType.IsImplemented<ISoftDeletable>())
            .Select(mutableType => builder.Entity(mutableType.ClrType));

        foreach (var entityTypeBuilder in entityTypeBuilders)
        {
            var queryFilterBuilder = entityTypeBuilder.AddQueryFilters();
            if (entityTypeBuilder.Metadata.ClrType.IsImplemented<IHasTenantId>())
                queryFilterBuilder.Add(x => EF.Property<string>(x, MultiTenantSettings.Instance.TenantIdFieldName) == tenantId);

            if (entityTypeBuilder.Metadata.ClrType.IsImplemented<ISoftDeletable>())
                queryFilterBuilder.Add(x => EF.Property<bool>(x, nameof(ISoftDeletable.Deleted)) == false);

            queryFilterBuilder.Build();
        }
    }
}
