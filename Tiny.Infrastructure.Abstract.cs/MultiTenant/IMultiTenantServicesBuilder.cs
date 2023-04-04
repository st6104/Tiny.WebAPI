// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace Tiny.Infrastructure.Abstract.MultiTenant;

public interface IMultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo>
    where TService : class, IMultiTenantService
    where TDbContext : DbContext, IMultiTenantManagerDbContext<TTenentInfo>
    where TStore : class, IMultiTenantStore<TTenentInfo>
    where TTenentInfo : class, ITenantInfo
{
    IServiceCollection Services { get; }

    IMultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo> AddDbContextOption(Action<DbContextOptionsBuilder> optionsAction);

    IServiceCollection Build();
}

public class MultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo> : IMultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo>
    where TService : class, IMultiTenantService
    where TDbContext : DbContext, IMultiTenantManagerDbContext<TTenentInfo>
    where TStore : class, IMultiTenantStore<TTenentInfo>
    where TTenentInfo : class, ITenantInfo
{
    private Action<DbContextOptionsBuilder>? _dbContextOptionsBuilderAction;
    
    public IServiceCollection Services { get; }

    internal static IMultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo> Create(
        IServiceCollection services)
    {
        return new MultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo>(services);
    }

    private MultiTenantServicesBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IMultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo> AddDbContextOption(Action<DbContextOptionsBuilder> optionsAction)
    {
        _dbContextOptionsBuilderAction = optionsAction;
        return this;
    }

    public IServiceCollection Build()
    {
        Services.AddScoped<IMultiTenantService, TService>();
        Services.AddDbContext<IMultiTenantManagerDbContext<TTenentInfo>, TDbContext>(_dbContextOptionsBuilderAction);
        Services.AddScoped<IMultiTenantStore<TTenentInfo>, TStore>();

        return Services;
    }
}
