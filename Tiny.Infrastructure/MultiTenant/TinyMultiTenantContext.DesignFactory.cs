// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Design;

namespace Tiny.Infrastructure.MultiTenant;

public class TinyMultiTenantContextDesignFactory : IDesignTimeDbContextFactory<TinyMultiTenantContext>
{
    public TinyMultiTenantContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TinyMultiTenantContext>().UseSqlServer(string.Empty, settings =>
        {
            settings.MigrationsAssembly(DbContextMigrationAssembly.Name);
        });
        return new TinyMultiTenantContext(optionsBuilder.Options);
    }
}
