namespace Tiny.Shared.DomainEntity;

public static class Constraint
{
    public static readonly NumericPrecision Precision = NumericPrecision.Default;
    public const int MaxLength = 200;
}

public class NumericPrecision
{
    public static readonly NumericPrecision Default = new(19, 6);

    public int Precision { get; }
    public int Scale { get; }

    public NumericPrecision(int precision, int scale)
    {
        Precision = precision;
        Scale = scale;
    }
}
