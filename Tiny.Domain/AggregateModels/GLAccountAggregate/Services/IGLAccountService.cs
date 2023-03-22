using Ardalis.Specification;
using Tiny.Shared.DomainService;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

public interface IGLAccountService : IDomainService
{
    Task CheckDuplicatedCodeAsync(string code, CancellationToken cancellationToken);
    Task CheckDuplicatedCodeAsync(string code, long excludedId, CancellationToken cancellationToken);
    void CheckValidAccountingType(GLAccount glAccount);
    void CheckValidPostable(GLAccount glAccount);
}