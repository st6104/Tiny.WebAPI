using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Exceptions;
using Tiny.Infrastructure.Extensions;
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

    public TinyDbContext(IMediator mediator, ILoggerFactory loggerFactory, IMultiTenantService multiTanentService) : base(multiTanentService, loggerFactory)
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

    protected override void ActionSqlServerOptions(SqlServerDbContextOptionsBuilder builder)
    {
        builder.EnableRetryOnFailure(RetryCount, TimeSpan.FromSeconds(5), null);
        builder.MigrationsAssembly(DbContextMigrationAssembly.Name);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetPreconventions();
    }

    private IReadOnlyList<EntityEntry<IHasDomainEvents>> GetAllEntityEntiriesWithDomainEvents()
    {
        return ChangeTracker.Entries<IHasDomainEvents>()
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
