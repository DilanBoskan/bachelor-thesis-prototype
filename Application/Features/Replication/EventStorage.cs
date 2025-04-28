using Application.Contracts.Event;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.Features.Replication;
public class EventStorage(ILogger<EventStorage> logger) : IEventHandler {
    private readonly ILogger<EventStorage> _logger = logger;
    public IReadOnlyList<Event> Events => _toBePublishedEvents;

    public Task HandleAsync(IReadOnlyList<Event> events) {
        _logger.LogDebug("Putting {Count} events in the queue to be published", events.Count);

        _toBePublishedEvents.AddRange(events);

        return Task.CompletedTask;
    }

    public void Clear(IReadOnlyList<Event> events) {
        _toBePublishedEvents.RemoveAll(events.Contains);
    }

    private readonly List<Event> _toBePublishedEvents = []; // TODO: Store locally
}