using Ardalis.SmartEnum;

namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class JournalEntryStatus : SmartEnum<JournalEntryStatus>
{
    public static readonly JournalEntryStatus Applied = new("신청", 1);
    public static readonly JournalEntryStatus Approved = new("승인", 2);
    public static readonly JournalEntryStatus Rejected = new("반려", 3);

    public JournalEntryStatus(string name, int value) : base(name, value)
    {
    }
}
