// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiny.QueryFilterBuilder;

internal class QueryFilterBuilder : QueryFilterBuilderAbstract<object>, IQueryFilterBuilder
{
    private readonly EntityTypeBuilder _builder;

    public static IQueryFilterBuilder Create(EntityTypeBuilder builder)
    {
        return new QueryFilterBuilder(builder);
    }

    private QueryFilterBuilder(EntityTypeBuilder builder) : base(builder)
    {
        _builder = builder;
    }

    public IQueryFilterBuilder Add(Expression<Func<object, bool>> filter)
    {
        AddCore(filter);
        return this;
    }

    public EntityTypeBuilder Build()
    {
        BuildCore();
        return _builder;
    }
}
