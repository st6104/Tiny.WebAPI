using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;

namespace Tiny.Infrastructure;

partial class TinyContext
{
    #region DbSets
    public virtual DbSet<AccountingType> AccountingType => Set<AccountingType>();
    public virtual DbSet<Postable> Postable => Set<Postable>();
    public virtual DbSet<GLAccount> GLAccount => Set<GLAccount>();
    public virtual DbSet<Department> Department => Set<Department>();
    public virtual DbSet<User> User => Set<User>();
    public virtual DbSet<JournalEntryStatus> JournalEntryStatus => Set<JournalEntryStatus>();
    public virtual DbSet<JournalEntry> JournalEntry => Set<JournalEntry>();
    public virtual DbSet<JournalEntryLine> JournalEntryLine => Set<JournalEntryLine>();
    #endregion
}