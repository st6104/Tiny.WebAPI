// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Ardalis.Specification;

namespace Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;

public class GetLastGLAccountIdTakeBy : Specification<GLAccount>, ISingleResultSpecification
{
    //TODO: Offset Zero Id Property 추가?
    public long? OffsetZeroGLAccountId { get; }
    public int TakeCount { get; }

    public GetLastGLAccountIdTakeBy(int takeCount, long? offSetZeroGLAccountId = null)
    {
        if (offSetZeroGLAccountId.HasValue && offSetZeroGLAccountId < 0)
            throw new ArgumentOutOfRangeException(nameof(offSetZeroGLAccountId),
                "Value cannot be less than 0.");

        OffsetZeroGLAccountId = offSetZeroGLAccountId;
        TakeCount = takeCount;
        Query.Where(x=> x.Id > OffsetZeroGLAccountId.GetValueOrDefault(0L)).OrderBy(x => x.Id).Take(TakeCount);
    }
}
