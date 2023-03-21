using Tiny.Shared.DomainService;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

public interface IGLAccountService : IDomainService
{
    Task<bool> IsExistingCode(string code, CancellationToken cancellationToken);

    Task<bool> IsExistingCode(long excludedId, string code, CancellationToken cancellationToken);
}