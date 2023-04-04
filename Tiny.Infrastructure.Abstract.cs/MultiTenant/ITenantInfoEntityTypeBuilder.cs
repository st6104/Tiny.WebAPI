// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiny.Infrastructure.Abstract.MultiTenant;

public class ITenantInfoEntityTypeBuilder<TTenantInfo> : IEntityTypeConfiguration<TTenantInfo> where TTenantInfo:class, ITenantInfo
{
    public void Configure(EntityTypeBuilder<TTenantInfo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(200);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ConnectionString).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(false);
    }
}
