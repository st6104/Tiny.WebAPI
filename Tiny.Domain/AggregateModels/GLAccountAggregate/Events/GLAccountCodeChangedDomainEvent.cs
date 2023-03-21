using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

public class GLAccountCodeChangedDomainEvent : DomainEvent, IValueChangedDomainEvent<string>
{
    public string Before { get; }

    public string After { get; }

    public GLAccountCodeChangedDomainEvent(string before, string after)
    {
        Before = before;
        After = after;
    }
}
