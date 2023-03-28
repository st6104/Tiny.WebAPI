// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Infrastructure.Abstract.Extensions;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract.SoftDelete;
using Tiny.Infrastructure.Abstract.Utilities;
using Tiny.Shared.DomainEntity;
using Tiny.Shared.Extensions;

namespace Tiny.Infrastructure.Abstract;
public abstract class EntityTypeConfigurationBase<T> : IEntityTypeConfiguration<T> where T : class
{
    private readonly ITenantInfo _currentTenant;

    protected EntityTypeConfigurationBase(ITenantInfo currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public void Configure(EntityTypeBuilder<T> builder)
    {   
        ConfigureEntity(builder);
        var entityType = typeof(T)!;
        var filters = new List<BinaryExpression>();
        var entityParamExp = Expression.Parameter(typeof(T), "entity");
        if (entityType.IsImplemented<IHasTenantId>())
        {
            builder.AddTenantIdProperty();
            var filterExpression = EFexpression.GetPropertyValueEqualityExpression(entityParamExp, TenantFieldNames.Id, _currentTenant.Id);
            if (filterExpression != null) filters.Add(filterExpression);
        }

        if (entityType.IsImplemented<ISoftDeletable>())
        {
            builder.AddDeletedAtProperty();
            var filterExpression = EFexpression.GetPropertyValueEqualityExpression(entityParamExp, nameof(ISoftDeletable.Deleted), false);
            if (filterExpression != null) filters.Add(filterExpression);
        }

        if (filters.Any() == false) return;
        //TODO : 좀 세련되게 쿼리필터를 걸 수 있는 방법 강구...
        var expression = filters.Aggregate(Expression.AndAlso);
        var queryFilter = Expression.Lambda<Func<T, bool>>(expression, entityParamExp);
        builder.HasQueryFilter(queryFilter);

    }

    public abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}
