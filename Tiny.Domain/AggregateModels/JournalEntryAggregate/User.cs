using Tiny.MultiTenant.Abstract.Interfaces;

namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class User : SoftDeletableEntity, IHasTenantId
{
    public string Code { get; }

    public string Name { get; }

    public User(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
