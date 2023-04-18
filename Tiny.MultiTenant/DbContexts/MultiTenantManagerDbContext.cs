// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tiny.MultiTenant.DbContexts.EntityTypeBuilders;
using Tiny.MultiTenant.Interfaces;

namespace Tiny.MultiTenant.DbContexts;
public abstract class MultiTenantManagerDbContext<TTenantInfo> : DbContext, IMultiTenantManagerDbContext<TTenantInfo> where TTenantInfo : class, ITenantInfo
{
    private IDbContextTransaction? _currentTransaction;
    
    public abstract DbSet<TTenantInfo> TenantInfo { get; }
    public bool HasTransaction => _currentTransaction is not null;

    protected MultiTenantManagerDbContext(DbContextOptions options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TenantInfoEntityTypeBuilder<TTenantInfo>());
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (HasTransaction)
            throw new TransactionException("Transaction already exist.");

        _currentTransaction = await this.Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
            throw new TransactionException("Transaction not exist.");

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction is not null)
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
            if (_currentTransaction is not null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
