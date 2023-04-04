// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Infrastructure.Abstract.MultiTenant;

public interface IMultiTenantStore<T> where T : class, ITenantInfo
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> TryGetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> TryAddAsync(T tenantInfo, CancellationToken cancellationToken = default);
    Task<bool> TryChangeName(string id, string name, CancellationToken cancellationToken = default);
    Task<bool> TryChangeConnectionString(string id, string connectionString, CancellationToken cancellationToken = default);
    //Task<bool> TryRemoveByIdAsync(string id);
    Task ActiveTenantAsync(string id, CancellationToken cancellationToken = default);
    Task InactiveTenantAsync(string id, CancellationToken cancellationToken = default);
}
