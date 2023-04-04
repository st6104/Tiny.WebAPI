// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Tiny.Infrastructure.Abstract.MultiTenant;

namespace Tiny.Infrastructure.MultiTenant;

public class TinyMultiTenantStore : MultiTenantStore<TenantInfo>
{
    public TinyMultiTenantStore(ILogger<TinyMultiTenantStore> logger,
        IMultiTenantManagerDbContext<TenantInfo> dbContext) : base(logger, dbContext)
    {
    }
}
