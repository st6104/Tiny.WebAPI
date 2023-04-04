// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiny.Infrastructure.Abstract.MultiTenant;

namespace Tiny.Infrastructure.MultiTenant;
public class TinyMultiTenantContext : MultiTenantManagerDbContext<TenantInfo>
{
    public override DbSet<TenantInfo> TenantInfo => Set<TenantInfo>();

    public TinyMultiTenantContext(DbContextOptions options) : base(options)
    {
    }
}
