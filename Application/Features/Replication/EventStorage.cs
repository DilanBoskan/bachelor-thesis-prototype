using Application.Contracts.Event;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.Features.Replication;
public class EventStorage(ILogger<EventStorage> logger) : IEventHandler {
    private readonly ILogger<EventStorage> _logger = logger;
    public IReadOnlyList<IEvent> Events => _toBePublishedEvents;

    public Task HandleAsync(IReadOnlyList<IEvent> events) {
        _logger.LogDebug("Putting {Count} events in the queue to be published", events.Count);

        _toBePublishedEvents.AddRange(events);

        return Task.CompletedTask;
    }

    public void Clear(IReadOnlyList<IEvent> events) {
        _toBePublishedEvents.RemoveAll(events.Contains);
    }

    private readonly List<IEvent> _toBePublishedEvents = []; // TODO: Store locally
}