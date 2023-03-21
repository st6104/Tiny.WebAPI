namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

/// <summary>
/// 계정과목 신규 추가 도메인 이벤트
/// </summary>
public class GLAccountAddedDomainEvent : DomainEvent
{
    public GLAccount GLAccount{ get; }

    public GLAccountAddedDomainEvent(GLAccount gLAccount)
    {
        GLAccount = gLAccount;
    }
}
