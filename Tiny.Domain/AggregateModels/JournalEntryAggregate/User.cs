using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class User : Entity
{
    public string Code { get; }

    public string Name { get; }

    public User(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
