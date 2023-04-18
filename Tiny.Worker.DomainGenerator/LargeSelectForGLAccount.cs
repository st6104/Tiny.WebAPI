// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Application.ViewModelExtensions;

namespace Tiny.Worker.DomainGenerator;

public class LargeSelectForGLAccount
{
    private readonly IGLAccountRepository _repository;

    public LargeSelectForGLAccount(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var models = await _repository.GetAll(cancellationToken).ToViewModel().ToListAsync(cancellationToken);
    }
}
