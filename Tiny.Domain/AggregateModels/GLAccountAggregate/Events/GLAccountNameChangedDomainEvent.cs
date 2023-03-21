namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Events;

/// <summary>
/// 계정과목명 변경에 대한 도메인 이벤트
/// </summary>
public class GLAccountNameChangedDomainEvent : DomainEvent, IValueChangedDomainEvent<string>
{
    public string Before { get; }

    public string After { get; }

    public GLAccountNameChangedDomainEvent(string before, string after)
    {
        Before = before;
        After = after;
    }
}