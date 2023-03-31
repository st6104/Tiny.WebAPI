// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Tiny.Infrastructure.Abstract.GlobalQueryFilters;
internal class QueryFilter<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, bool>> Expression { get; }

    public QueryFilter(Expression<Func<TEntity, bool>> expression)
    {
        Expression = expression;
    }
}
