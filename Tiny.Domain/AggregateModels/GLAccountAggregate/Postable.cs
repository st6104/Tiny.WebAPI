using Ardalis.SmartEnum;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate;

public class Postable : SmartEnum<Postable>
{
    public static readonly Postable Yes = new(nameof(Yes), 1);
    public static readonly Postable No = new(nameof(No), 2);

    public const int NameLength = 200;
    public Postable(string name, int value) : base(name, value)
    {
    }
}
