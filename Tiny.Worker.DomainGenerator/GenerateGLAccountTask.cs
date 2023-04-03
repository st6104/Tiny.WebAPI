// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoBogus;
using Bogus;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Shared.Extensions;

namespace Tiny.Worker.DomainGenerator;

public interface IGenerateGLAccountTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

internal class GenerateGLAccountTask : IGenerateGLAccountTask
{
    private readonly ILogger<GenerateGLAccountTask> _logger;
    private readonly IGLAccountRepository _repository;

    public GenerateGLAccountTask(ILogger<GenerateGLAccountTask> logger, IGLAccountRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var lorem = new Bogus.DataSets.Lorem(locale: "ko");

        var codeBase = 2000;
        var codeIndex = 1;
        var fakeGlAccounts = new AutoFaker<GLAccount>()
            .StrictMode(true)
            .RuleFor(o => o.Code, f => (codeBase + codeIndex++).ToString())
            .RuleFor(o => o.Name, f => lorem.Sentence(2))
            .RuleFor(o => o.PostableId, f => f.PickRandom<Postable>(Postable.List).Value)
            .RuleFor(o => o.AccountingTypeId, f => f.PickRandom<AccountingType>(AccountingType.List).Value)
            .Ignore(o => o.Id)
            .Ignore(o => o.AccountingType)
            .Ignore(o => o.Balance)
            .Ignore(o => o.Deleted);

        var generatedGlAccounts = fakeGlAccounts.Generate(200);

        await generatedGlAccounts.ForEachAsync(async (glAccount, ctk) => await _repository.AddAsync(glAccount, ctk), cancellationToken);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
