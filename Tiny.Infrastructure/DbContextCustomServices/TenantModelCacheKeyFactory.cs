using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Tiny.Infrastructure.DbContextCustomServices;

public class TenantModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
    {
        return new TenantModelCacheKey(context, designTime);
    }
}

public class TenantModelCacheKey : ModelCacheKey
{
    private readonly string _tenantId;

/*
    public TenantModelCacheKey(DbContext context) : base(context)
    {
        _tenantId = (context as TinyContext)?.TenantId ?? string.Empty;
    }
*/

    public TenantModelCacheKey(DbContext context, bool designTime) : base(context, designTime)
    {
        _tenantId = (context as TinyContext)?.TenantId ?? string.Empty;
    }

    protected override bool Equals(ModelCacheKey other)
    {
        if (other is not TenantModelCacheKey model)
        {
            return false;
        }

        return _tenantId == model._tenantId;
    }
}
