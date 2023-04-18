using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Events;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;
using Tiny.Shared.Exceptions;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure.Repositories;

public class GLAccountRepository : IGLAccountRepository
{
    private readonly TinyDbContext _dbContext;

    public GLAccountRepository(TinyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task AddAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(glAccount, cancellationToken);
        //glAccount.AddDomainEvent(new GLAccountAddedDomainEvent(glAccount));
    }

    public async Task UpdateAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            _dbContext.GLAccount.Attach(glAccount).State = EntityState.Modified;
        }, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var glAccount = await GetOneAsync(new GLAccountByIdSpec(id), cancellationToken);
        //glAccount.TryMarkAsDelete();
    }

    public IQueryable<GLAccount> GetAll(CancellationToken cancellationToken)
    {
        return GetCore(null).AsQueryable();
        //return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<GLAccount> GetTopN(int queryCount, int skipCount,
        CancellationToken cancellationToken)
    {
        //_dbContext.GLAccount.OrderBy(x=>x.Id).Take(skipCount)
        
        var lastGLAccountId = GetCore(new GetLastGLAccountIdTakeBy(skipCount)).Select(x => x.Id)
            .LastOrDefault();

        return GetCore(new GetLastGLAccountIdTakeBy(queryCount, lastGLAccountId)).AsQueryable();
    }

    public async Task<GLAccount> GetOneAsync(GLAccountByIdSpec spec, CancellationToken cancellationToken)
    {
        var glAccount = await GetCore(spec).FirstOrDefaultAsync(cancellationToken);
        return glAccount ?? throw new EntityIdNotFoundException(spec.Id);
    }

    public async Task<bool> IsSatisfiedByAsync(ISpecification<GLAccount> specification, CancellationToken cancellationToken)
    {
        return await _dbContext.GLAccount.WithSpecification(specification).AnyAsync(cancellationToken);
    }

    public IQueryable<GLAccount> GetCore(ISpecification<GLAccount>? spec)
    {
        var query = _dbContext.GLAccount.AsNoTracking();

        if (spec != null)
        {
            query = query.WithSpecification(spec);
        }

        return query;
    }
}
