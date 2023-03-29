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

namespace Tiny.Infrastructure;

public partial class TinyContext : DbContext, IUnitOfWork, IDomainEventStore
{
    public const string DefaultSchema = "dbo";
    private const string MigrationAssembly = "Tiny.Infrastructure.Migrations";
    private const int RetryCount = 3;
    private readonly ICurrentTenantInfo _currentTanent;
    private readonly ILoggerFactory _loggerFactory;

    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;

    public TinyContext(IMediator mediator, ILoggerFactory loggerFactory, ICurrentTenantInfo currentTanent)
    {
        _loggerFactory = loggerFactory;
        _currentTanent = currentTanent;
        _mediator = mediator;
    }

    public string TenantId => _currentTanent.Current.Id;

    public bool HasActiveTransaction => _currentTransaction != null;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        FillTenantIdToAddedEntities();
        FillDeletedAtToDeletedEntities();

        return base.SaveChangesAsync(cancellationToken);
    }

    #region IUnitOfWork Implements

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this, cancellationToken);

        await SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder.UseSqlServer(_currentTanent.Current.ConnectionString, options =>
            {
                options.EnableRetryOnFailure(RetryCount, TimeSpan.FromSeconds(5), null);
                options.MigrationsAssembly(MigrationAssembly);
            })
            .ReplaceService<IModelCacheKeyFactory, TenantModelCacheKeyFactory>();

        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetPreconventions();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyEntityConfigurationsFromAssemblyContaining<TinyContext>(_currentTanent.Current);
    }

    private void FillDeletedAtToDeletedEntities()
    {
        var addedOrModifiedEntityStatus = new[] { EntityState.Added, EntityState.Modified };
        var addedOrModifiedSoftDeleteEntries = ChangeTracker.Entries<ISoftDeletable>()
            .Where(entry => addedOrModifiedEntityStatus.Contains(entry.State)
                            && entry.Entity.Deleted)
            .ToList();

        addedOrModifiedSoftDeleteEntries.ForEach(entry =>
        {
            entry.Property(SoftDeleteFieldNames.DeletedAt).CurrentValue = DateTime.UtcNow;
        });
    }

    private void FillTenantIdToAddedEntities()
    {
        var addedOrModifiedTenantEntries = ChangeTracker.Entries<IHasTenantId>()
            .Where(entry => entry.State == EntityState.Added)
            .ToList();

        addedOrModifiedTenantEntries.ForEach(entry =>
            entry.Property(TenantFieldNames.Id).CurrentValue = _currentTanent.Current.Id);
    }

    private IReadOnlyList<EntityEntry<Entity>> GetAllEntityEntiriesWithDomainEvents()
    {
        return ChangeTracker.Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .ToList()
            .AsReadOnly();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            throw new TransactionAlreadyExistsException();
        }

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (transaction != _currentTransaction)
        {
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

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
}
