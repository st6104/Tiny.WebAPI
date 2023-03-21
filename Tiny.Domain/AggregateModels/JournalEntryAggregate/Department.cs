namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class Department : Entity
{
    public string Code { get; }

    public string Name { get; }

    public Department(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
