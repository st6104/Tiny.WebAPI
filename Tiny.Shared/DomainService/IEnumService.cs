using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace Tiny.Shared.DomainService;

public interface IEnumService<T>: IDomainService where T : SmartEnum<T>
{
    Task<bool> ExistsAsync(int value, CancellationToken cancellationToken);
}
