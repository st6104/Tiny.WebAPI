using Ardalis.SmartEnum;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate
{
    public class AccountingType : SmartEnum<AccountingType>
    {
        public static readonly AccountingType Asset = new(nameof(Asset), 1);
        public static readonly AccountingType Liability = new(nameof(Liability), 2);
        public static readonly AccountingType Equity = new(nameof(Equity), 3);
        public static readonly AccountingType Revenue = new(nameof(Revenue), 4);
        public static readonly AccountingType Expense = new(nameof(Expense), 5);

        public const int NameLength = 200;
        public AccountingType(string name, int value) : base(name, value)
        {
        }
    }
}
