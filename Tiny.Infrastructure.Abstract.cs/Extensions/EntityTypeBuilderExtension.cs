// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract.SoftDelete;
using Tiny.Shared.DomainEntity;

namespace Tiny.Infrastructure.Abstract.Extensions;
public static class EntityTypeBuilderExtension
{
    public static EntityTypeBuilder<TEntity> AddTenantIdProperty<TEntity>(this EntityTypeBuilder<TEntity> builder, bool isRequred = true) where TEntity : class
    {
        builder.Property<string>(TenantFieldNames.Id).IsRequired(isRequred);
        return builder;
    }

    public static EntityTypeBuilder<TEntity> AddDeletedAtProperty<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        builder.Property<DateTime?>(SoftDeleteFieldNames.DeletedAt);
        return builder;
    }
}
