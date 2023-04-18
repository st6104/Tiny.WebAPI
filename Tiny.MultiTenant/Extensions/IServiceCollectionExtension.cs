// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Tiny.MultiTenant.Interfaces;
using Tiny.MultiTenant.Services;

namespace Tiny.MultiTenant.Extensions;

public static class IServiceCollectionExtension
{
    public static IMultiTenantServiceBuilder<TTenantInfo>
        AddMultiTenantService<TTenantInfo>(this IServiceCollection services)
        where TTenantInfo : class, ITenantInfo
    {
        return MultiTenantServiceBuilder<TTenantInfo>.Create(services);
    }
}
