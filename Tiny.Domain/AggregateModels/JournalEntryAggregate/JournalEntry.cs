using System.Diagnostics.CodeAnalysis;
namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class JournalEntry : Entity, IAggregateRoot
{
    /// <summary>
    /// 전기일자
    /// </summary>
    public DateTime PostingDate { get; private set; }

    /// <summary>
    /// 전표상태
    /// </summary>
    public JournalEntryStatus Status { get; private set; } = null!;
    private int _journalEntryStatusId;

    /// <summary>
    /// 작성부서
    /// </summary>
    public Department Department { get; private set; } = null!;
    private long _departmentId;

    /// <summary>
    /// 적요
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 분개라인
    /// </summary>
    public IReadOnlyList<JournalEntryLine> Lines => _lines;
    private readonly List<JournalEntryLine> _lines = new();

    // /// <summary>
    // /// 작성자
    // /// </summary>
    // public User CreatedUser { get; private set; } = null!;
    // private int _createdUserId;

    // /// <summary>
    // /// 수정자
    // /// </summary>
    // public User? ModifiedUser { get; private set; }
    // private int? _modifiedUserId;

    public JournalEntry(DateTime postingDate, long departmentId, string description)
    {
        PostingDate = postingDate;
        _departmentId = departmentId;
        Description = description;
        _journalEntryStatusId = JournalEntryStatus.Applied.Value;

        // _createdUserId = createUserId;
    }

    public void AddLine(JournalEntryLine line)
    {
        _lines.Add(line);
    }

    public void RemoveLine(JournalEntryLine line)
    {
        _lines.Remove(line);
    }
}
