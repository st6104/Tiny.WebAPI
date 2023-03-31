using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;
using Tiny.Domain.Exceptions;

namespace Tiny.Application.DomainServices;
public class GLAccountService : IGLAccountService
{
    private readonly IGLAccountRepository _repository;

    public GLAccountService(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public void CheckValidPostable(GLAccount glAccount)
    {
        if (!Postable.List.Any(x => x.Value == glAccount.PostableId))
            throw new GLAccountValidationError(nameof(GLAccount.PostableId), "존재하지 않는 계정구분입니다.");
    }

    public void CheckValidAccountingType(GLAccount glAccount)
    {
        var spec = new GLAcountHasValidAccountingTypeSpec();
        if (!spec.IsSatisfiedBy(glAccount))
            throw new GLAccountValidationError(nameof(GLAccount.AccountingTypeId), "존재하지 않는 계정체계입니다.");
    }

    public async Task CheckDuplicatedCodeAsync(string code, CancellationToken cancellationToken)
    {
        await CheckDuplicatedCodeAsyncCore(code, null, cancellationToken);
    }

    public async Task CheckDuplicatedCodeAsync(string code, long excludedId, CancellationToken cancellationToken)
    {
        await CheckDuplicatedCodeAsyncCore(code, excludedId, cancellationToken);
    }

    private async Task CheckDuplicatedCodeAsyncCore(string code, long? excludedId, CancellationToken cancellationToken)
    {
        var existingCodeSpec = new GLAccountCodeExistSpec(code, excludedId);
        if (await _repository.IsSatisfiedByAsync(existingCodeSpec, cancellationToken))
            throw new GLAccountValidationError(nameof(GLAccount.Code), "이미 존재하는 계정코드입니다.");
    }
}
