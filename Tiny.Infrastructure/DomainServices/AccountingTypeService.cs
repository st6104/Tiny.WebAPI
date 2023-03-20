using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

namespace Tiny.Infrastructure.DomainServices;

public class AccountingTypeService : IAccountingTypeService
{
    public Task<bool> ExistsAsync(int value, CancellationToken cancellationToken)
    {
        return Task.Run(() => AccountingType.TryFromValue(value, out _));
    }
}
