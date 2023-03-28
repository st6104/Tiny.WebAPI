using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Tiny.Infrastructure.DbContextCustomServices;

//public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
//{
//    public object Create(DbContext context, bool designTime)
//    {
//       return new DynamicModelCacheKey(context, designTime);
//    }
//}

//public class DynamicModelCacheKey : ModelCacheKey
//{
//    private readonly string _tenantId;

//    public DynamicModelCacheKey(DbContext context) : base(context)
//    {
//        _tenantId = (context as TinyContext)?.Tenant?.Id ?? string.Empty;
//    }

//    public DynamicModelCacheKey(DbContext context, bool designTime) : base(context, designTime)
//    {
//        _tenantId = (context as TinyContext)?.Tenant?.Id ?? string.Empty;
//    }

//    protected override bool Equals(ModelCacheKey other)
//    {
//        if(other == null || other is not DynamicModelCacheKey model) return false;

//        return this._tenantId == model._tenantId;
//    }
//}
