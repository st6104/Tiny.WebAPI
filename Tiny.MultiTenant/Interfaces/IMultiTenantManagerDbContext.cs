// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Tiny.MultiTenant.Interfaces;
public interface IMultiTenantManagerDbContext<TTenantInfo> where TTenantInfo : class, ITenantInfo
{
    DbSet<TTenantInfo> TenantInfo { get; }
    
    bool HasTransaction { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    void RollbackTransaction();
}
