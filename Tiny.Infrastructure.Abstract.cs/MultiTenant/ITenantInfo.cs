using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.Infrastructure.Abstract.MultiTenant
{
    public interface ITenantInfo
    {
        public string Id { get; }
        public string Name { get; }
        public string ConnectionString { get; }
    }
}
