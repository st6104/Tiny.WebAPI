namespace Tiny.Shared.DomainEvent;

public interface IDomainEventStore
{
    IReadOnlyList<IDomainEvent> GetAll();

    void Clear();
}