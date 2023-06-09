using Ardalis.Specification;

namespace Tiny.Shared.Repository;

public interface IRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }

    Task<bool> IsSatisfiedByAsync(ISpecification<T> specification, CancellationToken cancellationToken);
}
