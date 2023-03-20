using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class JournalEntryLine : Entity
{
    public GLAccount GLAccount { get; } = null!;
    private long _gLAccountId;

    /// <summary>
    /// 차변금액
    /// </summary>
    public decimal DebitAmount { get; private set; }

    /// <summary>
    /// 대변금액
    /// </summary>
    public decimal CreditAmount { get; private set; }

    /// <summary>
    /// 적요
    /// </summary>
    public string Description { get; } = string.Empty;

    public JournalEntryLine(long gLAccountId, decimal debitAmount, decimal creditAmount, string description)
    {
        _gLAccountId = gLAccountId;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;
        Description = description;
    }

    public void ChangeGLAccount(long glAccountId)
    {
        _gLAccountId = glAccountId;
    }

    public void ChangeDebitAmount(decimal debitAmount)
    {
        DebitAmount = debitAmount;
    }

    public void ChangeCreditAmount(decimal creditAmount)
    {
        CreditAmount = creditAmount;
    }
}
