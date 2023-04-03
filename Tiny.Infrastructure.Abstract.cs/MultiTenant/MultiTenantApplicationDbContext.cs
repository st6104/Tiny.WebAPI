// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Tiny.Infrastructure.Abstract.Extensions;
using Tiny.Infrastructure.Abstract.SoftDelete;
using Tiny.Shared.DomainEntity;
using Tiny.Shared.Extensions;

namespace Tiny.Infrastructure.Abstract.MultiTenant;

public abstract class MultiTenantApplicationDbContext : DbContext
{
    private readonly IMultiTenantService _multiTenantService;
    private readonly ILoggerFactory _loggerFactory;

    public string TenantId => _multiTenantService.Current.Id;
    protected string TenantConncectionString => _multiTenantService.Current.ConnectionString;

    protected MultiTenantApplicationDbContext(IMultiTenantService multiTenantService, ILoggerFactory loggerFactory)
    {
        _multiTenantService = multiTenantService;
        _loggerFactory = loggerFactory;
    }

    protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.UseSqlServer(TenantConncectionString, ActionSqlServerOptions);
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, TenantModelCacheKeyFactory>();

        if (IsLoggerEnabled())
            optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected virtual bool IsLoggerEnabled()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }

    protected abstract void ActionSqlServerOptions(SqlServerDbContextOptionsBuilder builder);

    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnBeforeModelCreating(modelBuilder);
        modelBuilder.ApplyEntityConfigurationsFromAssemblyContaining(this.GetType());

        var mutableEntityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var mutableEntityType in mutableEntityTypes)
        {
            var entityTypeBuilder = modelBuilder.Entity(mutableEntityType.ClrType);
            var queryFilterBullder = entityTypeBuilder.AddQueryFilters();

            if (mutableEntityType.ClrType.IsImplemented<IHasTenantId>())
            {
                if (mutableEntityType.IsExistProperty(TenantFieldNames.Id))
                    entityTypeBuilder.AddTenantIdProperty();

                queryFilterBullder.Add(x => EF.Property<string>(x, TenantFieldNames.Id) == TenantId);
            }

            if (mutableEntityType.ClrType.IsImplemented<ISoftDeletable>())
            {
                if (mutableEntityType.IsExistProperty(SoftDeleteFieldNames.DeletedAt))
                    entityTypeBuilder.AddDeletedAtProperty();

                queryFilterBullder.Add(x => EF.Property<bool>(x, nameof(ISoftDeletable.Deleted)) == false);
            }

            queryFilterBullder.Build();
        }
    }

    protected virtual void OnBeforeModelCreating(ModelBuilder modelBuilder) { }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        FillTenantIdToAddedEntities();
        FillDeletedAtToDeletedEntities();

        return base.SaveChangesAsync(cancellationToken);
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
            entry.Property(TenantFieldNames.Id).CurrentValue = _multiTenantService.Current.Id);
    }
}
