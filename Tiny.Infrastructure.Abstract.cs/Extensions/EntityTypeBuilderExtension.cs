// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tiny.Infrastructure.Abstract.GlobalQueryFilters;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract.SoftDelete;
using Tiny.Shared.DomainEntity;

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

    public static KeyBuilder HasKeyWithTenantId<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object?>> keyExpression) where TEntity : class, IHasTenantId
    {
        if (!builder.Metadata.IsExistProperty(TenantFieldNames.Id))
            builder.AddTenantIdProperty();

        var propertyNames = keyExpression.GetPropertyNamesFrom().ToArray();
        var keyColumnNames = new string[propertyNames.Length + 1];
        keyColumnNames[0] = TenantFieldNames.Id;
        Array.Copy(propertyNames.ToArray(), 0, keyColumnNames, 1, propertyNames.Length);

        return builder.HasKey(keyColumnNames);
    }

    public static IndexBuilder<TEntity> HasIndexWithTenantId<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IHasTenantId
    {
        if (!builder.Metadata.IsExistProperty(TenantFieldNames.Id))
            builder.AddTenantIdProperty();

        return builder.HasIndex(TenantFieldNames.Id);
    }

    public static IndexBuilder<TEntity> HasIndexWithTenantId<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object?>> propertyExpression) where TEntity : class, IHasTenantId
    {
        if (!builder.Metadata.IsExistProperty(TenantFieldNames.Id))
            builder.AddTenantIdProperty();

        var propertyNames = propertyExpression.GetPropertyNamesFrom().ToArray();
        var indexColumnNames = new string[propertyNames.Length + 1];
        indexColumnNames[0] = TenantFieldNames.Id;
        Array.Copy(propertyNames.ToArray(), 0, indexColumnNames, 1, propertyNames.Length);

        return builder.HasIndex(indexColumnNames.ToArray());
    }

    private static IEnumerable<string> GetPropertyNamesFrom<TEntity>(this Expression<Func<TEntity, object?>> expression) where TEntity : class, IHasTenantId
    {
        var propertyNames = new List<string>();

        if (expression.Body is NewExpression newExp)
            propertyNames.AddRange(newExp.Members?.Select(member => member.Name) ?? Array.Empty<string>());
        else if (expression.Body is MemberExpression memberExp)
            propertyNames.Add(memberExp.Member.Name);
        else if (expression.Body is UnaryExpression unaryExp &&
                    unaryExp.Operand is MemberExpression memberExpInUnary)
            propertyNames.Add(memberExpInUnary.Member.Name);
        else
            throw new ArgumentException("Do not acceptable expression Type");

        return propertyNames;
    }
}
