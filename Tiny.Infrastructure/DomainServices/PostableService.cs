using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

namespace Tiny.Infrastructure.DomainServices;

public class PostableService : IPostableService
{
    public Task<bool> ExistsAsync(int value, CancellationToken cancellationToken)
    {
        return Task.Run(() => Postable.TryFromValue(value, out var postable));
    }
}
