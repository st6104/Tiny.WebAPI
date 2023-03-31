using Ardalis.SmartEnum;

namespace Tiny.Shared.DomainService;

public interface IEnumService<T>: IDomainService where T : SmartEnum<T>
{
    Task<bool> ExistsAsync(int value, CancellationToken cancellationToken);
}
