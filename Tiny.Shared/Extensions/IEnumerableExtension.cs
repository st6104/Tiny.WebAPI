namespace Tiny.Shared.Extensions;

public static class IEnumerableExtensions
{
    public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> func, CancellationToken cancellationToken)
    {
        foreach (var item in source)
        {
            if(cancellationToken.IsCancellationRequested)
                return;

            await func(item);
        }
    }
}