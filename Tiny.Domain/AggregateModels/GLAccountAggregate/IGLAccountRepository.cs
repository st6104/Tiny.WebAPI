using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate;

public interface IGLAccountRepository : IRepository<GLAccount>
{
    Task<GLAccount> GetOneAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<GLAccount>> GetAllAsync(CancellationToken cancellationToken);
    Task<GLAccount> AddAsync(GLAccount glAccount, CancellationToken cancellationToken);
    Task<GLAccount> UpdateAsync(GLAccount glAccount, CancellationToken cancellationToken);
    Task<GLAccount> ChangeNameAsync(long id, string name, CancellationToken cancellationToken);
    Task<GLAccount> ChangeAccoutingTypeAsync(long id, int accountingTypeId, CancellationToken cancellationToken);
    Task<GLAccount> ChangeBalanceAsync(long id, decimal balance, CancellationToken cancellationToken);
}
