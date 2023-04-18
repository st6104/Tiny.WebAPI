using Tiny.MultiTenant.Abstract.Interfaces;

namespace Tiny.Shared.DomainEntity;

public interface IAggregateRoot : ISoftDeletable, IHasTenantId
{
}
