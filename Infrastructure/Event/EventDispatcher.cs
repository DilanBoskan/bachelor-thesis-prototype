using Application.Contracts.Event;
using Domain.Events;

namespace Infrastructure.Event;
internal sealed class EventDispatcher(IEnumerable<IEventHandler> eventHandlers) : IEventDispatcher {
    private readonly IReadOnlyCollection<IEventHandler> _eventHandlers = eventHandlers.ToList();

    async Task IEventDispatcher.PublishAsync(IReadOnlyList<IEvent> events, CancellationToken ct) {
        ArgumentNullException.ThrowIfNull(events);
        if (events.Count == 0) return;

        foreach (var eventHandler in _eventHandlers) {
            await eventHandler.HandleAsync(events);
        }
    }
}
