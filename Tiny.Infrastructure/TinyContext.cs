using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract.SoftDelete;
using Tiny.Infrastructure.DbContextCustomServices;
using Tiny.Infrastructure.Exceptions;
using Tiny.Infrastructure.Extensions;
using Tiny.Shared.DomainEntity;
using Tiny.Shared.DomainEvent;
using Tiny.Shared.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tiny.Infrastructure;

public partial class TinyContext : DbContext, IUnitOfWork, IDomainEventStore
{
    public const string DefaultSchema = "dbo";
    private const string MigrationAssembly = "Tiny.Infrastructure.Migrations";
    private const int Retry_Count = 3;

    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ICurrentTenantInfo _currentTanent;

    public TinyContext(IMediator mediator, ILoggerFactory loggerFactory, ICurrentTenantInfo currentTanent) : base()
    {
        _loggerFactory = loggerFactory;
        _currentTanent = currentTanent;
        _mediator = mediator;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && _currentTanent.Current != null)
        {
            optionsBuilder.UseSqlServer(_currentTanent.Current.ConnectionString, options =>
            {
                options.EnableRetryOnFailure(maxRetryCount: Retry_Count, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                options.MigrationsAssembly(MigrationAssembly);

            });

            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetPreconventions();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyEntityConfigurationsFromAssemblyContaining<TinyContext>(_currentTanent.Current);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {//TODO : 테스트 필요
        var addedOrModifiedEntityStatus = new EntityState[] { EntityState.Added, EntityState.Modified };

        var addedOrModifiedTenantEntries = ChangeTracker.Entries<IHasTenantId>()
                                                        .Where(entry => addedOrModifiedEntityStatus.Any(status => status == entry.State))
                                                        .ToList();

        addedOrModifiedTenantEntries.ForEach(entry => entry.Property(TenantFieldNames.Id).CurrentValue = _currentTanent.Current.Id);

        var addedOrModifiedSoftDeleteEntries = ChangeTracker.Entries<ISoftDeletable>()
                                                            .Where(entry => addedOrModifiedEntityStatus.Any(status => status == entry.State)
                                                                            && entry.Entity.Deleted)
                                                            .ToList();

        addedOrModifiedSoftDeleteEntries.ForEach(entry =>
        {
            entry.Property(SoftDeleteFieldNames.DeletedAt).CurrentValue = DateTime.UtcNow;
        });


        return base.SaveChangesAsync(cancellationToken);
    }

    #region IDomainEventStore Implements
    public IReadOnlyList<IDomainEvent> GetAll()
    {
        return GetAllEntityEntiriesWithDomainEvents()
                                .SelectMany(entiry => entiry.Entity.DomainEvents)
                                .ToList()
                                .AsReadOnly();
    }

    public void Clear()
    {
        GetAllEntityEntiriesWithDomainEvents()
            .ToList()
            .ForEach(entry => entry.Entity.ClearDomainEvents());
    }
    #endregion

    #region IUnitOfWork Implements
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this, cancellationToken);

        await this.SaveChangesAsync(cancellationToken);

        return true;
    }
    #endregion

    private IReadOnlyList<EntityEntry<Entity>> GetAllEntityEntiriesWithDomainEvents()
    {
        return ChangeTracker.Entries<Entity>()
                                .Where(entry => entry.Entity.DomainEvents.Any())
                                .ToList()
                                .AsReadOnly();
    }

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) throw new TransactionAlreadyExistsException();

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
