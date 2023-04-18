// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tiny.MultiTenant.Interfaces;
using Tiny.MultiTenant.Middlewares;

namespace Tiny.MultiTenant.Extensions;

public static class IApplicataionBuilderExtension
{
    public static IApplicationBuilder UseMultiTenantMiddleware(this IApplicationBuilder builder,
        string requestHeaderName)
    {
        var multiTenantSettings =
            builder.ApplicationServices.GetRequiredService<IMultiTenantSettings>();
        return builder.UseMiddleware(multiTenantSettings.MiddlewareType, requestHeaderName);
    }
}
