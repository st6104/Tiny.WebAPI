namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

public class GLAccountTypeChangedDomainEvent : DomainEvent, IValueChangedDomainEvent<int>
{
    public int Before { get; set; }
    public int After { get; set; }

    public GLAccountTypeChangedDomainEvent(int before, int after)
    {
        Before = before;
        After = after;
    }
}