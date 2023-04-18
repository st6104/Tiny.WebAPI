// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.MultiTenant.Abstract.Interfaces;
using Tiny.MultiTenant.Services;
using Tiny.Shared.DomainEntity;

namespace Tiny.MultiTenant.Extensions;

public static class EntityTypeBuilderExtension
{
    internal static EntityTypeBuilder<TEntity> AddTenantIdProperty<TEntity>(this EntityTypeBuilder<TEntity> builder, string fieldName,
        bool isRequred = true) where TEntity : class
    {
        ((EntityTypeBuilder)builder).AddTenantIdProperty(fieldName, isRequred);
        return builder;
    }

    internal static EntityTypeBuilder AddTenantIdProperty(this EntityTypeBuilder builder, string fieldName, bool isRequred = true)
    {
        builder.Property<string>(fieldName).IsRequired(isRequred);
        return builder;
    }

    public static EntityTypeBuilder<TEntity> AddDeletedAtProperty<TEntity>(this EntityTypeBuilder<TEntity> builder, string fieldName)
        where TEntity : class
    {
        ((EntityTypeBuilder)builder).AddDeletedAtProperty(fieldName);
        return builder;
    }

    public static EntityTypeBuilder AddDeletedAtProperty(this EntityTypeBuilder builder, string fieldName)
    {
        builder.Property<DateTime?>(fieldName);
        return builder;
    }

    public static KeyBuilder HasKeyWithTenantId<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object?>> keyExpression) where TEntity : class, IHasTenantId
    {
        if (!builder.Metadata.IsExistProperty(MultiTenantSettings.Instance.TenantIdFieldName))
            builder.AddTenantIdProperty(MultiTenantSettings.Instance.TenantIdFieldName);

        var propertyNames = keyExpression.GetPropertyNamesFrom().ToArray();
        var keyColumnNames = new string[propertyNames.Length + 1];
        keyColumnNames[0] = MultiTenantSettings.Instance.TenantIdFieldName;
        Array.Copy(propertyNames.ToArray(), 0, keyColumnNames, 1, propertyNames.Length);

        return builder.HasKey(keyColumnNames);
    }

    public static IndexBuilder<TEntity> HasIndexWithTenantId<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IHasTenantId
    {
        if (!builder.Metadata.IsExistProperty(MultiTenantSettings.Instance.TenantIdFieldName))
            builder.AddTenantIdProperty(MultiTenantSettings.Instance.TenantIdFieldName);

        return builder.HasIndex(MultiTenantSettings.Instance.TenantIdFieldName);
    }

    public static IndexBuilder<TEntity> HasIndexWithTenantId<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object?>> propertyExpression) where TEntity : class, IHasTenantId
    {
        if (!builder.Metadata.IsExistProperty(MultiTenantSettings.Instance.TenantIdFieldName))
            builder.AddTenantIdProperty(MultiTenantSettings.Instance.TenantIdFieldName);

        var propertyNames = propertyExpression.GetPropertyNamesFrom().ToArray();
        var indexColumnNames = new string[propertyNames.Length + 1];
        indexColumnNames[0] = (MultiTenantSettings.Instance.TenantIdFieldName);
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
