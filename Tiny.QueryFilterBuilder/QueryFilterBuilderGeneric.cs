// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiny.QueryFilterBuilder;

internal class QueryFilterBuilderGeneric<TEntity> : QueryFilterBuilderAbstract<TEntity>, IQueryFilterBuilder<TEntity> where TEntity : class
{
    private readonly EntityTypeBuilder<TEntity> _builder;

    public static IQueryFilterBuilder<TEntity> Create(EntityTypeBuilder<TEntity> builder)
    {
        return new QueryFilterBuilderGeneric<TEntity>(builder);
    }

    private QueryFilterBuilderGeneric(EntityTypeBuilder<TEntity> builder) : base(builder)
    {
        _builder = builder;
    }

    public IQueryFilterBuilder<TEntity> Add(Expression<Func<TEntity, bool>> filter)
    {
        AddCore(filter);
        return this;
    }

    public EntityTypeBuilder<TEntity> Build()
    {
        BuildCore();
        return _builder;
    }
}
