using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

namespace Tiny.Infrastructure.DomainServices;

public class GLAccountTypeService : IGLAccountService
{
    private readonly TinyContext _dbContext;

    public GLAccountTypeService(TinyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> IsExistingCode(string code, CancellationToken cancellationToken)
    {
        return _dbContext.GLAccount.AnyAsync(x => x.Code == code, cancellationToken);
    }

    public Task<bool> IsExistingCode(long excludedId, string code, CancellationToken cancellationToken)
    {
        return _dbContext.GLAccount.AnyAsync(x => x.Id != excludedId && x.Code == code, cancellationToken);
    }
}
