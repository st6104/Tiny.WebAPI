// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Infrastructure.Abstract.MultiTenant;

public interface IMultiTenantStore<T> where T : class, ITenantInfo
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> TryGetByIdAsync(string id, out T tenantInfo, CancellationToken cancellationToken = default);
    //Task<bool> TryAddAsync(T tenantInfo);
    //Task<bool> TryUpdateAsync(T tenantInfo);
    //Task<bool> TryRemoveByIdAsync(string id);
}
