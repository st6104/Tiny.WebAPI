using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Tiny.MultiTenant.DbContexts;

public class ApplicationDbContextCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
    {
        return new ApplicationDbContextCacheKey(context, designTime);
    }
}

public class ApplicationDbContextCacheKey : ModelCacheKey
{
    private readonly string _tenantId;

/*
    public TenantModelCacheKey(DbContext context) : base(context)
    {
        _tenantId = (context as TinyContext)?.TenantId ?? string.Empty;
    }
*/

    public ApplicationDbContextCacheKey(DbContext context, bool designTime) : base(context, designTime)
    {
        _tenantId = (context as MultiTenantApplicationDbContext)?.TenantId ?? string.Empty;
    }

    protected override bool Equals(ModelCacheKey other)
    {
        if (other is not ApplicationDbContextCacheKey model)
        {
            return false;
        }

        return _tenantId == model._tenantId;
    }
}
