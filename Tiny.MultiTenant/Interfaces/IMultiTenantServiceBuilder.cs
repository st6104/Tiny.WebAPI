// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tiny.MultiTenant.DbContexts;

namespace Tiny.MultiTenant.Interfaces;

public interface IMultiTenantServiceBuilder<TTenantInfo> where TTenantInfo : class, ITenantInfo
{
    IServiceCollection Services { get; }

    IMultiTenantServiceBuilder<TTenantInfo> UseTenentIdField(bool useField = true);

    IMultiTenantServiceBuilder<TTenantInfo> AddTenentIdField(string fieldName);

    IMultiTenantServiceBuilder<TTenantInfo> AddTenantDbContext<TDbContext>(
        Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : MultiTenantManagerDbContext<TTenantInfo>;

    // IMultiTenantServiceBuilder<TTenantInfo> AddTenantStore<TDbContext>()
    //     where TDbContext : IMultiTenantManagerDbContext<TTenantInfo>;

    IMultiTenantServiceBuilder<TTenantInfo> AddApplicationDbContext<TDbContext>()
        where TDbContext : MultiTenantApplicationDbContext;

    IMultiTenantServiceBuilder<TTenantInfo> AddMultiTenantService<TService>()
        where TService : class, IMultiTenantService;

    IMultiTenantServiceBuilder<TTenantInfo> UseDistributedCache();
}
