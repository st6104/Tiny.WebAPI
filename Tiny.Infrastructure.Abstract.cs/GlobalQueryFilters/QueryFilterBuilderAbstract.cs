// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiny.Infrastructure.Abstract.GlobalQueryFilters;
internal abstract class QueryFilterBuilderAbstract<TEntity> where TEntity : class
{
    private readonly EntityTypeBuilder _builder;
    private readonly IList<QueryFilter<TEntity>> _queryFilters = new List<QueryFilter<TEntity>>();

    protected QueryFilterBuilderAbstract(EntityTypeBuilder builder)
    {
        _builder = builder;
    }

    protected void AddCore(Expression<Func<TEntity, bool>> filter)
    {
        _queryFilters.Add(new QueryFilter<TEntity>(filter));
    }

    protected void BuildCore()
    {
        if (!_queryFilters.Any()) return;

        var entityParameter = Expression.Parameter(_builder.Metadata.ClrType, "entity");

        var filterBody = _queryFilters.Select(queryFilter =>
        {
            var filterParameter = queryFilter.Expression.Parameters[0];
            var visitor = new ReplaceParameterVisitor(filterParameter, entityParameter);
            return visitor.Visit(queryFilter.Expression.Body);
        }).Aggregate(Expression.AndAlso);

        var expression = Expression.Lambda(filterBody, entityParameter);

        _builder.HasQueryFilter(expression);
    }
}
