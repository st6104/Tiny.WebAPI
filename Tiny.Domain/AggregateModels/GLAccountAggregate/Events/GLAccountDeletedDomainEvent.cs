namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

public class GLAccountDeletedDomainEvent : DomainEvent
{
    public GLAccount GLAccount{ get; }

    public GLAccountDeletedDomainEvent(GLAccount gLAccount)
    {
        GLAccount = gLAccount;
    }
}
