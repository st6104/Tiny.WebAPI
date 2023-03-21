namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

public class GLAccountBalanceChangedDomainEvent : DomainEvent, IValueChangedDomainEvent<decimal>
{
    public decimal Before { get; }

    public decimal After { get; }

    public GLAccountBalanceChangedDomainEvent(decimal before, decimal after)
    {
        Before = before;
        After = after;
    }
}