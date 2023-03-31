using Tiny.Domain.Exceptions;

namespace Tiny.Domain.AggregateModels.JournalEntryAggregate;

public class JournalEntry : SoftDeletableEntity, IAggregateRoot
{
    /// <summary>
    /// 전기일자
    /// </summary>
    public DateTime PostingDate { get; private set; }

    /// <summary>
    /// 전표상태
    /// </summary>
    public JournalEntryStatus Status { get; private set; } = null!;
    public int JournalEntryStatusId { get; private set; }

    /// <summary>
    /// 작성부서
    /// </summary>
    public Department Department { get; private set; } = null!;
    public long DepartmentId { get; private set; }

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
        DepartmentId = departmentId;
        Description = description;
        JournalEntryStatusId = JournalEntryStatus.Applied.Value;

        // _createdUserId = createUserId;
    }

    public JournalEntry ChangeDepartment(long departmentId)
    {
        if (DepartmentId == departmentId)
            return this;

        if (!IsDepartmentChangePossible())
            throw new JournalEntryValidationError(nameof(DepartmentId), "[신청] 상태의 문서만 부서변경이 가능합니다.");

        DepartmentId = departmentId;

        return this;
    }

    private bool IsDepartmentChangePossible()
    {
        return JournalEntryStatusId == JournalEntryStatus.Applied.Value;
    }

    public JournalEntry ChangeJournalStatusToApproved()
    {
        if (IsTransient())
            throw new JournalEntryValidationError(nameof(JournalEntryStatusId), "신규문서는 문서상태를 변경할 수 없습니다.");

        if (JournalEntryStatusId == JournalEntryStatus.Approved.Value)
            throw new JournalEntryValidationError(nameof(JournalEntryStatusId), "이미 승인된 문서입니다.");

        if (JournalEntryStatusId == JournalEntryStatus.Rejected.Value)
            throw new JournalEntryValidationError(nameof(JournalEntryStatusId), "이미 반려된 문서입니다.");

        JournalEntryStatusId = JournalEntryStatus.Approved.Value;

        return this;
    }

    public JournalEntry ChangeJournalStatusToRejected()
    {
        if (IsTransient())
            throw new JournalEntryValidationError(nameof(JournalEntryStatusId), "신규문서는 문서상태를 변경할 수 없습니다.");

        if (JournalEntryStatusId == JournalEntryStatus.Approved.Value)
            throw new JournalEntryValidationError(nameof(JournalEntryStatusId), "이미 승인된 문서입니다.");

        if (JournalEntryStatusId == JournalEntryStatus.Rejected.Value)
            throw new JournalEntryValidationError(nameof(JournalEntryStatusId), "이미 반려된 문서입니다.");

        JournalEntryStatusId = JournalEntryStatus.Rejected.Value;

        return this;
    }

    public JournalEntry SetDescription(string description)
    {
        if (Description == description)
            return this;

        Description = description;

        return this;
    }

    public JournalEntryLine AddLine(long gLAccountId, decimal debitAmount, decimal creditAmount, string description)
    {//TODO : 승인상태에 따른 추가여부 결정
        var line = new JournalEntryLine(gLAccountId, debitAmount, creditAmount, description);
        _lines.Add(line);
        return line;
    }

    public JournalEntry RemoveLine(JournalEntryLine line)
    {//TODO : 승인상태에 따른 추가여부 결정
        _lines.Remove(line);
        return this;
    }

    public override bool TryMarkAsDelete()
    {
        if (IsTransient() || Deleted) return false;

        if (JournalEntryStatusId == JournalEntryStatus.Applied.Value)
            throw new JournalEntryValidationError("", "[승인]된 문서는 삭제할 수 없습니다.");

        Deleted = true;
        return true;
    }
}
