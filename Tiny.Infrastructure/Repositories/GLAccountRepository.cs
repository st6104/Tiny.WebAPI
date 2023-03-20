using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Shared.Exceptions;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure.Repositories;

public class GLAccountRepository : IGLAccountRepository
{
    private readonly TinyContext _dbContext;
    public IUnitOfWork UnitOfWork => _dbContext;

    public GLAccountRepository(TinyContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<GLAccount> AddAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(glAccount, cancellationToken);

        await UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return glAccount;
    }

    public async Task<GLAccount> UpdateAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        _dbContext.Entry(glAccount).State = EntityState.Modified;

        await UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return await GetOneAsync(glAccount.Id, cancellationToken);
    }

    public async Task<GLAccount> ChangeAccoutingTypeAsync(long id, int accountingTypeId, CancellationToken cancellationToken)
    {
        AccountingType.TryFromValue(accountingTypeId, out var accountingType);

        var glAccount = await GetOneAsync(id, cancellationToken);
        glAccount.ChangeType(accountingType);
        await UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return glAccount;
    }

    public async Task<GLAccount> ChangeBalanceAsync(long id, decimal balance, CancellationToken cancellationToken)
    {
        var glAccount = await GetOneAsync(id, cancellationToken);
        glAccount.ChangeBalance(balance);
        await UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return glAccount;
    }

    public async Task<GLAccount> ChangeNameAsync(long id, string name, CancellationToken cancellationToken)
    {
        var glAccount = await GetOneAsync(id, cancellationToken);
        glAccount.ChangeName(name);
        await UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return glAccount;
    }

    public async Task<IReadOnlyList<GLAccount>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetCore(true).ToListAsync(cancellationToken);
    }

    public async Task<GLAccount> GetOneAsync(long id, CancellationToken cancellationToken)
    {
        var glAccount = await GetCore(true).FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        if (glAccount is null)
            throw new IdNotFoundException();

        return glAccount;
    }

    public IQueryable<GLAccount> GetCore(bool withNavigationProperties)
    {
        var query = _dbContext.GLAccount.AsQueryable();

        if (withNavigationProperties)
        {
            query = query.Include(x => x.AccountingType)
                        .Include(x => x.Postable);
        }

        return query;
    }
}
