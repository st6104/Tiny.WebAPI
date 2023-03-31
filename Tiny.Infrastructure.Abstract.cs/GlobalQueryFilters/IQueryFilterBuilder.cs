// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiny.Infrastructure.Abstract.GlobalQueryFilters;

public interface IQueryFilterBuilder
{
    public IQueryFilterBuilder Add(Expression<Func<object, bool>> filter);
    public EntityTypeBuilder Build();
}

public interface IQueryFilterBuilder<TEntity> where TEntity : class
{
    public IQueryFilterBuilder<TEntity> Add(Expression<Func<TEntity, bool>> filter);
    public EntityTypeBuilder<TEntity> Build();
}
