namespace Tiny.Shared.DomainEvent;

public interface IValueChangedDomainEvent<T>: IDomainEvent
{
    T Before{ get; }
    T After { get; }
}