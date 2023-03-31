using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;
using Tiny.Shared.Exceptions;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure.Repositories;

public class GLAccountRepository : IGLAccountRepository
{
    private readonly TinyDbContext _dbDbContext;

    public GLAccountRepository(TinyDbContext dbDbContext)
    {
        _dbDbContext = dbDbContext;
    }

    public IUnitOfWork UnitOfWork => _dbDbContext;

    public async Task AddAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        await _dbDbContext.AddAsync(glAccount, cancellationToken);
    }

    public async Task UpdateAsync(GLAccount glAccount, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            _dbDbContext.GLAccount.Attach(glAccount).State = EntityState.Modified;
        }, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var glAccount = await GetOneAsync(new GLAccountByIdSpec(id), cancellationToken);
        glAccount.TryMarkAsDelete();
    }

    public async Task<IReadOnlyList<GLAccount>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetCore(null).ToListAsync(cancellationToken);
    }

    public async Task<GLAccount> GetOneAsync(GLAccountByIdSpec spec, CancellationToken cancellationToken)
    {
        var glAccount = await GetCore(spec).FirstOrDefaultAsync(cancellationToken);
        return glAccount ?? throw new EntityIdNotFoundException(spec.Id);
    }

    public Task<bool> IsSatisfiedByAsync(ISpecification<GLAccount> specification, CancellationToken cancellationToken)
    {
        return _dbDbContext.GLAccount.WithSpecification(specification).AnyAsync(cancellationToken);
    }

    public IQueryable<GLAccount> GetCore(ISpecification<GLAccount>? spec)
    {
        var query = _dbDbContext.GLAccount.AsQueryable();

        if (spec != null)
        {
            query = query.WithSpecification(spec);
        }

        return query;
    }
}
