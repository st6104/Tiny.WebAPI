using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Tiny.Infrastructure.Exceptions;
using Tiny.Infrastructure.Extensions;
using Tiny.MultiTenant.DbContexts;
using Tiny.MultiTenant.Interfaces;
using Tiny.Shared.DomainEntity;
using Tiny.Shared.DomainEvent;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure;

public partial class TinyDbContext : MultiTenantApplicationDbContext, IUnitOfWork, IDomainEventStore
{
    public const string DefaultSchema = "dbo";
    private const int RetryCount = 3;

    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;

    public TinyDbContext(IMediator mediator, ILoggerFactory loggerFactory, IMultiTenantService multiTanentService,
        IMultiTenantSettings multiTenantSettings) : base(multiTanentService, loggerFactory, multiTenantSettings)
    {
        _mediator = mediator;
    }

    public bool HasActiveTransaction => _currentTransaction != null;

    #region IUnitOfWork Implements

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return true;
    }

    #endregion

    protected override void OnUseDatabase(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder.UseSqlServer(connectionString, settings =>
        {
            settings.EnableRetryOnFailure(RetryCount, TimeSpan.FromSeconds(5), null);
            settings.MigrationsAssembly(DbContextMigrationAssembly.Name);
            settings.MaxBatchSize(1000);
        });
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetPreconventions();
    }

    private IReadOnlyList<EntityEntry<IHasDomainEvents>> GetAllEntityEntiriesWithDomainEvents()
    {
        return FilteringEntriesHasDomainEvents(ChangeTracker.Entries<IHasDomainEvents>());
    }

    private  List<EntityEntry<IHasDomainEvents>> FilteringEntriesHasDomainEvents(
        IEnumerable<EntityEntry<IHasDomainEvents>> entityEntries)
    {
        var entriesAsArray = entityEntries.ToArray();
        var entityEntriesAsDomainEvents = new List<EntityEntry<IHasDomainEvents>>(entriesAsArray.Length);
        foreach (var entityEntry in entriesAsArray)
        {
            if(entityEntry.Entity.DomainEvents.Any())
                entityEntriesAsDomainEvents.Add(entityEntry);
        }
        
        entityEntriesAsDomainEvents.TrimExcess();
        return entityEntriesAsDomainEvents;
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
