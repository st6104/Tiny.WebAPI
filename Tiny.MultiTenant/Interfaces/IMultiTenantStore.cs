// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.MultiTenant.Interfaces;

public interface IMultiTenantStore<TTenantInfo> where TTenantInfo : class, ITenantInfo
{
    Task<IEnumerable<TTenantInfo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TTenantInfo?> TryGetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> TryAddAsync(TTenantInfo tenantInfo, CancellationToken cancellationToken = default);
    Task<bool> TryChangeName(string id, string name, CancellationToken cancellationToken = default);
    Task<bool> TryChangeConnectionString(string id, string connectionString, CancellationToken cancellationToken = default);
    //Task<bool> TryRemoveByIdAsync(string id);
    Task ActiveTenantAsync(string id, CancellationToken cancellationToken = default);
    Task InactiveTenantAsync(string id, CancellationToken cancellationToken = default);
}
