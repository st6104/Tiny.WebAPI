// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Tiny.MultiTenant.Abstract.Interfaces;
using Tiny.MultiTenant.Extensions;
using Tiny.MultiTenant.Interfaces;
using Tiny.MultiTenant.Services;

namespace Tiny.MultiTenant.DbContexts;

public abstract class MultiTenantApplicationDbContext : DbContext
{
    private readonly IMultiTenantService _multiTenantService;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMultiTenantSettings _multiTenantSettings;

    public string TenantId => _multiTenantService.Current.Id;
    protected string TenantConncectionString => _multiTenantService.Current.ConnectionString;

    protected MultiTenantApplicationDbContext(IMultiTenantService multiTenantService,
        ILoggerFactory loggerFactory,
        IMultiTenantSettings multiTenantSettings)
    {
        _multiTenantService = multiTenantService;
        _loggerFactory = loggerFactory;
        _multiTenantSettings = multiTenantSettings;
    }

    protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        OnUseDatabase(optionsBuilder, TenantConncectionString);
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, ApplicationDbContextCacheKeyFactory>();

        if (IsLoggerEnabled())
            optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected abstract void OnUseDatabase(DbContextOptionsBuilder optionsBuilder,
        string connectionString);

    protected virtual bool IsLoggerEnabled()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }

    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnBeforeModelCreating(modelBuilder);
        modelBuilder.ApplyEntityConfigurationsFromAssemblyContaining(this.GetType());

        if (MultiTenantSettings.Instance.UseTenantIdField)
            modelBuilder.AddTenantIdProperty(MultiTenantSettings.Instance.TenantIdFieldName);

        //TODO : softDelete?
        //modelBuilder.AddDeletedAtProperty();
        //TODO : TenantId와 SoftDelete(DeledtedAt)의 조합?
        modelBuilder.AddQueryFilter(TenantId);
    }

    protected virtual void OnBeforeModelCreating(ModelBuilder modelBuilder) { }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_multiTenantSettings.UseTenantIdField)
            FillTenantIdToAddedEntities();
    
        //TODO : SoftDelete는 어디에 위치해야할까,..?
        //FillDeletedAtToDeletedEntities();
    
        return base.SaveChangesAsync(cancellationToken);
    }

    //TODO : SoftDelete는 어디에 위치해야할까,..?
    // private void FillDeletedAtToDeletedEntities()
    // {
    //     var addedOrModifiedEntityStatus = new[] { EntityState.Added, EntityState.Modified };
    //     var addedOrModifiedSoftDeleteEntries = ChangeTracker.Entries<ISoftDeletable>()
    //         .Where(entry => addedOrModifiedEntityStatus.Contains(entry.State)
    //                         && entry.Entity.Deleted)
    //         .ToList();
    //
    //     addedOrModifiedSoftDeleteEntries.ForEach(entry =>
    //     {
    //         entry.Property(SoftDeleteFieldNames.DeletedAt).CurrentValue = DateTime.UtcNow;
    //     });
    // }

    private void FillTenantIdToAddedEntities()
    {
        var addedOrModifiedTenantEntries = FilteringByState(ChangeTracker.Entries<IHasTenantId>(), EntityState.Added);
        
        addedOrModifiedTenantEntries.ForEach(entry =>
            entry.Property(_multiTenantSettings.TenantIdFieldName).CurrentValue = "1000");
    }

    private List<EntityEntry<IHasTenantId>> FilteringByState(
        IEnumerable<EntityEntry<IHasTenantId>> entityEntries, EntityState entityState)
    {
        var entriesAsArray = entityEntries.ToArray();
        var filteredEntries = new List<EntityEntry<IHasTenantId>>(entriesAsArray.Length);
        foreach (var entityEntry in entriesAsArray)
        {
            if (entityEntry.State == entityState)
                filteredEntries.Add(entityEntry);
        }

        filteredEntries.TrimExcess();
        return filteredEntries;
    }
}
