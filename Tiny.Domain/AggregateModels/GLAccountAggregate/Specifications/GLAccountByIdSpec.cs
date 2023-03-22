using Ardalis.Specification;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;

public class GLAccountByIdSpec : Specification<GLAccount>, ISingleResultSpecification
{
    public long Id{ get; }

    public GLAccountByIdSpec(long id)
    {
        Id = id;
        Query.Where(x => x.Id == Id)
            .AsNoTracking()
            .Include(x=>x.Postable)
            .Include(x=>x.AccountingType);
    }
}
