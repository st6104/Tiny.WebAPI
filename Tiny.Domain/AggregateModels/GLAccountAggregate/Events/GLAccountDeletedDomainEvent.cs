using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

public class GLAccountDeletedDomainEvent : DomainEvent
{
    public GLAccount GLAccount{ get; }

    public GLAccountDeletedDomainEvent(GLAccount gLAccount)
    {
        GLAccount = gLAccount;
    }
}
