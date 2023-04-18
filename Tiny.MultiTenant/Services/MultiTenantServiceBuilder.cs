// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tiny.MultiTenant.DbContexts;
using Tiny.MultiTenant.Interfaces;
using Tiny.MultiTenant.Middlewares;

namespace Tiny.MultiTenant.Services;

internal class MultiTenantServiceBuilder<TTenantInfo> : IMultiTenantServiceBuilder<TTenantInfo> where TTenantInfo : class, ITenantInfo
{
    private readonly IServiceCollection _services;
    private readonly MultiTenantSettings _multiTenantSettings;

    public IServiceCollection Services => _services;

    public static MultiTenantServiceBuilder<TTenantInfo> Create(IServiceCollection services)
    {
        return new MultiTenantServiceBuilder<TTenantInfo>(services);
    }

    private MultiTenantServiceBuilder(IServiceCollection services)
    {
        _services = services;

        if (_services.Any(serviceDesc =>
                serviceDesc.ServiceType.Equals(typeof(IMultiTenantSettings))))
            throw new InvalidOperationException(
                "MultiTenant Services have already been registered.");

        _multiTenantSettings = MultiTenantSettings.Instance;
        _multiTenantSettings.MiddlewareType = typeof(MultiTenantMiddleware<TTenantInfo>);
        _services.AddSingleton<IMultiTenantSettings, MultiTenantSettings>(_ =>
            MultiTenantSettings.Instance);

        _services.AddMemoryCache();
    }

    public IMultiTenantServiceBuilder<TTenantInfo> UseTenentIdField(bool useField = true)
    {
        _multiTenantSettings.UseTenantIdField = useField;
        return this;
    }

    public IMultiTenantServiceBuilder<TTenantInfo> AddTenentIdField(string fieldName)
    {
        _multiTenantSettings.TenantIdFieldName = fieldName;
        return this;
    }

    public IMultiTenantServiceBuilder<TTenantInfo>
        AddTenantDbContext<TDbContext>(Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : MultiTenantManagerDbContext<TTenantInfo>
    {
        _services.AddDbContext<IMultiTenantManagerDbContext<TTenantInfo>, TDbContext>(
            optionsAction);
        
        _services
            .AddSingleton<IMultiTenantStore<TTenantInfo>, MultiTenantStore<TDbContext, TTenantInfo>>();
        return this;
    }

    // public IMultiTenantServiceBuilder<TTenantInfo> AddTenantStore<TDbContext>()
    //     where TDbContext : IMultiTenantManagerDbContext<TTenantInfo>
    // {
    //     // _services
    //     //     .AddScoped<IMultiTenantStore<TTenantInfo>, MultiTenantStore<TDbContext, TTenantInfo>>();
    //     return this;
    // }

    public IMultiTenantServiceBuilder<TTenantInfo> AddApplicationDbContext<TDbContext>()
        where TDbContext : MultiTenantApplicationDbContext
    {
        _services.AddDbContext<TDbContext>();
        return this;
    }

    public IMultiTenantServiceBuilder<TTenantInfo> AddMultiTenantService<TService>()
        where TService : class, IMultiTenantService
    {
        _services.AddScoped<IMultiTenantService, TService>();
        return this;
    }

    public IMultiTenantServiceBuilder<TTenantInfo> UseDistributedCache()
    {
        _multiTenantSettings.UseDistributedCache = true;
        return this;
    }
}
