// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Tiny.Application.ViewModelExtensions;
using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.Handlers.Queries;

public record GLAccountGetTopNQuery
    (int Count, int Skip) : IQueryRequest<IReadOnlyList<GLAccountViewModel>>;

public class
    GLAccountGetTopNQueryHandler : IRequestHandler<GLAccountGetTopNQuery,
        IReadOnlyList<GLAccountViewModel>>
{
    private readonly IGLAccountRepository _repository;

    public GLAccountGetTopNQueryHandler(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<GLAccountViewModel>> Handle(GLAccountGetTopNQuery request,
        CancellationToken cancellationToken)
    {
        return (await _repository.GetTopN(request.Count, request.Skip, cancellationToken)
            .ToViewModel().ToListAsync(cancellationToken)).AsReadOnly();
    }
}
