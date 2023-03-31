using Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate;

/// <summary>
/// 계정과목
/// </summary>
public class GLAccount : SoftDeletableEntity, IAggregateRoot
{
    public const int CodeLength = 50;
    public const int NameLength = 100;
    private const decimal InitialBalance = decimal.Zero;

    /// <summary>
    /// 계정코드
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// 계정명
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 분개가능계정 여부
    /// </summary>
    public Postable Postable { get; } = null!;
    public int PostableId { get; private set; }

    /// <summary>
    /// 계정분류
    /// </summary>
    public AccountingType AccountingType { get; private set; } = null!;
    public int AccountingTypeId { get; private set; }

    /// <summary>
    /// 계정잔액
    /// </summary>
    public decimal Balance { get; private set; }

    public GLAccount(string code, string name, int postableId, int accountingTypeId)
    {
        Code = code;
        Name = name;
        PostableId = postableId;
        AccountingTypeId = accountingTypeId;
        Balance = InitialBalance;

        AddDomainEvent(new GLAccountAddedDomainEvent(this));
    }

    public GLAccount ChangeCode(string code)
    {
        if (!IsTransient() && code != Code)
            AddDomainEvent(new GLAccountCodeChangedDomainEvent(Code, code));

        Code = code;

        return this;
    }

    public GLAccount ChangeName(string name)
    {
        if (!IsTransient() && name != Name)
            AddDomainEvent(new GLAccountNameChangedDomainEvent(Name, name));

        Name = name;

        return this;
    }

    public GLAccount ChangeType(int accountingTypeId)
    {
        if (!IsTransient() && accountingTypeId != AccountingTypeId)
            AddDomainEvent(new GLAccountTypeChangedDomainEvent(AccountingTypeId, accountingTypeId));

        AccountingTypeId = accountingTypeId;

        if (AccountingType.TryFromValue(accountingTypeId, out var accountingType))
            AccountingType = accountingType;

        return this;
    }

    public GLAccount ChangeBalance(decimal balance)
    {
        if (!IsTransient() && balance != Balance)
            AddDomainEvent(new GLAccountBalanceChangedDomainEvent(Balance, balance));

        Balance = balance;

        return this;
    }

    public override bool TryMarkAsDelete()
    {
        var markedSuccessed = base.TryMarkAsDelete();
        if (markedSuccessed)
            AddDomainEvent(new GLAccountDeletedDomainEvent(this));

        return markedSuccessed;
    }
}
