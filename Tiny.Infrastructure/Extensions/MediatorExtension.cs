using Tiny.Shared.DomainEvent;
using Tiny.Shared.Extensions;

namespace Tiny.Infrastructure.Extensions;

internal static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, IDomainEventStore eventStore,
        CancellationToken cancellationToken)
    {
        var domainEvents = eventStore.GetAll();
        await domainEvents.ForEachAsync(async (domainEvent, ctk) => await mediator.Publish(domainEvent, ctk), cancellationToken);
    }
}
