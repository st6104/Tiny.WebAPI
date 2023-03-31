namespace Tiny.Shared.DomainEvent;

public interface IValueChangedDomainEvent<out T>: IDomainEvent
{
    T Before{ get; }
    T After { get; }
}
