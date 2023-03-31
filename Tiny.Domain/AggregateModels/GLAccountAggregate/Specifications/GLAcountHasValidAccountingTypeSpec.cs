using Ardalis.Specification;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;

public class GLAcountHasValidAccountingTypeSpec : Specification<GLAccount>
{
    public GLAcountHasValidAccountingTypeSpec()
    {
        Query.Where(glAccount => AccountingType.List.Any(acctType => acctType.Value == glAccount.AccountingTypeId));
    }
}
