// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.Infrastructure.Abstract.MultiTenant
{
    public interface IMultiTenantStore<T> where T : class, ITenantInfo
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> TryGetByIdAsync(string id, out T tenantInfo);
        //Task<bool> TryAddAsync(T tenantInfo);
        //Task<bool> TryUpdateAsync(T tenantInfo);
        //Task<bool> TryRemoveByIdAsync(string id);
    }
}
