using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Behaviors;
using Tiny.Infrastructure.ConfigurationOptions;
using Tiny.Infrastructure.MultiTenant;
using Tiny.MultiTenant.Extensions;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure;

public static class ConfigureServiceContainer
{
    public static IServiceCollection AddServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<MarkedAssemlbyClass>()
                .WithRepositories();
        });

        services.AddMediatR(config =>
        {
            config
                .RegisterServicesFromAssemblyContaining<MarkedAssemlbyClass>();
            config.AddBehavior(typeof(IPipelineBehavior<,>),
                typeof(TransactionBehavior<,>));
        });

        var tenantManageDbConnectionString =
            configuration.GetConnectionString("TenantManage");

        services.AddMultiTenantService<TenantInfo>()
            .AddTenentIdField(TenantFieldNames.Id)
            .UseTenentIdField()
            .AddTenantDbContext<TinyMultiTenantContext>(options =>
            {
                options.UseSqlServer(tenantManageDbConnectionString,
                    settings =>
                    {
                        settings.MigrationsAssembly(DbContextMigrationAssembly
                            .Name);
                    });
            })
            .UseDistributedCache()
            .AddApplicationDbContext<TinyDbContext>()
            .AddMultiTenantService<MultiTenantService>();

        var redisOptions = configuration.GetSection(RedisCacheConfigOptions.SectionName)
            .Get<RedisCacheConfigOptions>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisOptions?.Configurtion;
            options.InstanceName = redisOptions?.InstanceName;
        });

        return services;
    }
}

internal static class ScrutorExtension
{
    public static IImplementationTypeSelector WithRepositories(
        this IImplementationTypeSelector selector)
    {
        return selector
            .AddClasses(type => type.AssignableTo(typeof(IRepository<>)))
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
