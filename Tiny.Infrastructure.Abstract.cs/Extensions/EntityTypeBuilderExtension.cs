// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Infrastructure.Abstract.GlobalQueryFilters;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract.SoftDelete;

namespace Tiny.Infrastructure.Abstract.Extensions;

public static class EntityTypeBuilderExtension
{
    public static EntityTypeBuilder<TEntity> AddTenantIdProperty<TEntity>(this EntityTypeBuilder<TEntity> builder,
        bool isRequred = true) where TEntity : class
    {
        ((EntityTypeBuilder)builder).AddTenantIdProperty(isRequred);
        return builder;
    }

    public static EntityTypeBuilder AddTenantIdProperty(this EntityTypeBuilder builder, bool isRequred = true)
    {
        builder.Property<string>(TenantFieldNames.Id).IsRequired(isRequred);
        return builder;
    }

    public static EntityTypeBuilder<TEntity> AddDeletedAtProperty<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        ((EntityTypeBuilder)builder).AddDeletedAtProperty();
        return builder;
    }

    public static EntityTypeBuilder AddDeletedAtProperty(this EntityTypeBuilder builder)
    {
        builder.Property<DateTime?>(SoftDeleteFieldNames.DeletedAt);
        return builder;
    }

    public static IQueryFilterBuilder<TEntity> AddQueryFilters<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        return QueryFilterBuilderGeneric<TEntity>.Create(builder);
    }

    public static IQueryFilterBuilder AddQueryFilters(this EntityTypeBuilder builder)
    {
        return QueryFilterBuilder.Create(builder);
    }
}
