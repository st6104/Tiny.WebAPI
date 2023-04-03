using Tiny.Shared.DomainEvent;

namespace Tiny.Shared.DomainEntity;

public abstract class Entity : IHasDomainEvents
{
    private int? _requestedHashCode;
    private readonly List<IDomainEvent> _domainEvents = new();

    public virtual long Id { get; protected set; }
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// 아직 저장되지 않은 엔티티 여부
    /// </summary>
    /// <returns></returns>
    public bool IsTransient() => this.Id == default;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity)
            return false;

        if (object.ReferenceEquals(this, obj))
            return true;

        if (!this.GetType().Equals(obj.GetType()))
            return false;

        if (this.IsTransient() || entity.IsTransient())
            return false;

        return this.Id == entity.Id;
    }

    public override int GetHashCode()
    {
        if (this.IsTransient())
            return base.GetHashCode();

        if (!_requestedHashCode.HasValue)
            _requestedHashCode = HashCode.Combine(this.Id.GetHashCode());

        return _requestedHashCode.Value;
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (object.Equals(left, null))
            return object.Equals(right, null);
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}
