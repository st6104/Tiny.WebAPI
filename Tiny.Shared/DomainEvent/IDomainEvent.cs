namespace Tiny.Shared.DomainEvent;

public interface IDomainEvent : INotification
{
    string Id { get; }
}