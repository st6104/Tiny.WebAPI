// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Infrastructure.Abstract.MultiTenant;

namespace Tiny.Infrastructure.MultiTenant;

public class MultiTenantStore : IMultiTenantStore<TenantInfo>
{
    private readonly List<TenantInfo> _tenants;

    public MultiTenantStore()
    {//TODO : 추후 DB에서 읽어오는 것으로 분리
        _tenants = new List<TenantInfo>
        {
            new("1000", "Tenant1",
                "Server=tinydb;Database=TinyWebJournal;User Id=sa;Password=wnsdlf83!!;MultipleActiveResultSets=true;Encrypt=false"),
            new("2000", "Tenant2",
                "Server=tinydb;Database=TinyWebJournal;User Id=sa;Password=wnsdlf83!!;MultipleActiveResultSets=true;Encrypt=false"),
            new("3000", "Tenant3",
                "Server=tinydb;Database=TinyWebJournal2;User Id=sa;Password=wnsdlf83!!;MultipleActiveResultSets=true;Encrypt=false")
        };
    }

    public Task<IEnumerable<TenantInfo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_tenants.AsEnumerable());
    }

    public Task<bool> TryGetByIdAsync(string id, out TenantInfo tenantInfo, CancellationToken cancellationToken = default)
    {
        tenantInfo = default!;
        var firstOrDefault = _tenants.FirstOrDefault(tenant => tenant.Id == id);
        if (firstOrDefault == null)
        {
            return Task.FromResult(false);
        }

        tenantInfo = firstOrDefault;
        return Task.FromResult(true);
    }

    //public Task<bool> TryAddAsync(TenantInfo tenantInfo)
    //{
    //    if (_tenants.Any(x => x.Id == tenantInfo.Id))
    //        return Task.FromResult(false);

    //    _tenants.Add(tenantInfo);
    //    return Task.FromResult(true);
    //}

    //public Task<bool> TryRemoveByIdAsync(string id)
    //{
    //    var tenant = _tenants.FirstOrDefault(tenant => tenant.Id == id);
    //    if (tenant == null) return Task.FromResult(false);

    //    return Task.FromResult(_tenants.Remove(tenant));
    //}

    //public Task<bool> TryUpdateAsync(TenantInfo tenantInfo)
    //{
    //    return Task.FromResult(false);
    //}
}
