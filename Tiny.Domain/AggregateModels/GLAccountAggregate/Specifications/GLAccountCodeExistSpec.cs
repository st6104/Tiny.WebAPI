using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;

public class GLAccountCodeExistSpec : SingleResultSpecification<GLAccount, bool>
{
    public GLAccountCodeExistSpec(string code, long? excludedId = null)
    {
        Query.Where(x => x.Code == code);

        if(excludedId.HasValue)
            Query.Where(x => x.Id != excludedId);
    }
}
