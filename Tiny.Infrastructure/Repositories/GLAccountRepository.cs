using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;
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

    public async Task AddAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(glAccount, cancellationToken);
    }

    public async Task UpdateAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            var entry = _dbContext.Entry(glAccount);

            _dbContext.GLAccount.Attach(glAccount).State = EntityState.Modified;
        }, cancellationToken);
    }

     public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var glAccount = await GetOneAsync(new GLAccountByIdSpec(id), cancellationToken);
        glAccount.MarkAsDelete();
    }

    public async Task<IReadOnlyList<GLAccount>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetCore(null).ToListAsync(cancellationToken);
    }

    public async Task<GLAccount> GetOneAsync(GLAccountByIdSpec spec, CancellationToken cancellationToken)
    {
        var glAccount = await GetCore(spec).FirstOrDefaultAsync(cancellationToken);
        if (glAccount is null)
            throw new IdNotFoundException(spec.Id);

        return glAccount;
    }

    public IQueryable<GLAccount> GetCore(ISpecification<GLAccount>? spec)
    {
        var query = _dbContext.GLAccount.AsQueryable();

        if (spec != null)
            query = query.WithSpecification(spec);

        return query;
    }

   
    public Task<bool> IsSatisfiedByAsync(ISpecification<GLAccount> specification, CancellationToken cancellationToken)
    {
        return _dbContext.GLAccount.WithSpecification(specification).AnyAsync(cancellationToken);
    }

    // public async Task<GLAccount> ChangeAccoutingTypeAsync(long id, int accountingTypeId, CancellationToken cancellationToken)
    // {
    //     AccountingType.TryFromValue(accountingTypeId, out var accountingType);

    //     var glAccount = await GetOneAsync(new GLAccountByIdSpec(id), cancellationToken);
    //     glAccount.ChangeType(accountingType);
    //     return glAccount;
    // }

    // public async Task<GLAccount> ChangeBalanceAsync(long id, decimal balance, CancellationToken cancellationToken)
    // {
    //     var glAccount = await GetOneAsync(new GLAccountByIdSpec(id), cancellationToken);
    //     glAccount.ChangeBalance(balance);
    //     return glAccount;
    // }

    // public async Task<GLAccount> ChangeNameAsync(long id, string name, CancellationToken cancellationToken)
    // {
    //     var glAccount = await GetOneAsync(new GLAccountByIdSpec(id), cancellationToken);
    //     glAccount.ChangeName(name);
    //     return glAccount;
    // }
}