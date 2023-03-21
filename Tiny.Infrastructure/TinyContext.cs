using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Tiny.Application.Interfaces;
using Tiny.Infrastructure.Exceptions;
using Tiny.Infrastructure.Extensions;
using Tiny.Shared.DomainEntity;
using Tiny.Shared.DomainEvent;
using Tiny.Shared.Repository;

namespace Tiny.Infrastructure;

public partial class TinyContext : DbContext, IUnitOfWork, IDomainEventStore
{
    public const string Default_Schema = "dbo";
    private const int Retry_Count = 3;

    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;
    private readonly IDbConnectionStore _connectionStringStore;
    private readonly ILoggerFactory _loggerFactory;

    public TinyContext(IDbConnectionStore connectionStringStore, IMediator mediator, ILoggerFactory loggerFactory) : base()
    {
        this._loggerFactory = loggerFactory;
        this._mediator = mediator;
        this._connectionStringStore = connectionStringStore;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionStringStore.Default, options =>
            {
                options.EnableRetryOnFailure(maxRetryCount: Retry_Count, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                options.MigrationsAssembly("Tiny.Infrastructure.Migrations");
            });

            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyEntityConfigurationsFromAssemblyContaining<TinyContext>();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetPreconventions();
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

        await base.SaveChangesAsync(cancellationToken);

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