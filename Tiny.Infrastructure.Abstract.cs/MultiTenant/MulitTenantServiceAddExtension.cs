// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace Tiny.Infrastructure.Abstract.MultiTenant;

public static class MulitTenantServiceAddExtension
{
    public static IMultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo> AddMultiTenantServices<TService, TDbContext, TStore, TTenentInfo>(this IServiceCollection serivces)
        where TService : class, IMultiTenantService
        where TDbContext : DbContext, IMultiTenantManagerDbContext<TTenentInfo>
        where TStore : class, IMultiTenantStore<TTenentInfo>
        where TTenentInfo : class, ITenantInfo
    {
        return MultiTenantServicesBuilder<TService, TDbContext, TStore, TTenentInfo>.Create(serivces);
    }
}
