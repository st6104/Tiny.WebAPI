using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class JournalEntryLine : Entity
{
    public GLAccount GLAccount { get; } = null!;
    public long GLAccountId { get; private set; }

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
    public string Description { get; private set; } = string.Empty;

    internal JournalEntryLine(long gLAccountId, decimal debitAmount, decimal creditAmount, string description)
    {
        GLAccountId = gLAccountId;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;
        Description = description;
    }

    public JournalEntryLine ChangeGLAccount(long glAccountId)
    {
        if (GLAccountId == glAccountId)
            return this;

        GLAccountId = glAccountId;

        return this;
    }

    public JournalEntryLine ChangeDebitAmount(decimal debitAmount)
    {
        if(DebitAmount == debitAmount)
            return this;

        DebitAmount = debitAmount;

        return this;
    }

    public JournalEntryLine ChangeCreditAmount(decimal creditAmount)
    {
        if(CreditAmount == creditAmount)
            return this;

        CreditAmount = creditAmount;

        return this;
    }

    public JournalEntryLine SetDescription(string description)
    {
        if(Description == description)
            return this;

        Description = description;

        return this;
    }
}
