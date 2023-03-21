using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;

namespace Tiny.Infrastructure;

partial class TinyContext
{
    #region DbSets
    public DbSet<AccountingType> AccountingType => Set<AccountingType>();
    public DbSet<Postable> Postable => Set<Postable>();
    public DbSet<GLAccount> GLAccount => Set<GLAccount>();
    public DbSet<Department> Department => Set<Department>();
    public DbSet<User> User => Set<User>();
    public DbSet<JournalEntryStatus> JournalEntryStatus => Set<JournalEntryStatus>();
    public DbSet<JournalEntry> JournalEntry => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLine => Set<JournalEntryLine>();
    #endregion
}