namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class Department : SoftDeletableEntity, IHasTenantId
{
    public string Code { get; }

    public string Name { get; }

    public Department(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
