using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate;

public interface IGLAccountRepository : IRepository<GLAccount>
{
    Task<GLAccount> GetOneAsync(GLAccountByIdSpec spec, CancellationToken cancellationToken);
    IQueryable<GLAccount> GetAll(CancellationToken cancellationToken);
    IQueryable<GLAccount> GetTopN(int queryCount, int skipCount,
        CancellationToken cancellationToken);
    Task AddAsync(GLAccount glAccount, CancellationToken cancellationToken);
    Task UpdateAsync(GLAccount glAccount, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
