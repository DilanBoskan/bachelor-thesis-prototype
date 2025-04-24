using Application.Contracts.Event;
using Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cloud;
public class EventAggregator(ILogger<EventAggregator> logger) : IEventHandler {
    private readonly ILogger<EventAggregator> _logger = logger;
    public IReadOnlyList<IEvent> Events => _toBePublishedEvents;

    public Task HandleAsync(IReadOnlyList<IEvent> events) {
        _logger.LogDebug("Putting {Count} events in the queue to be published", events.Count);

        _toBePublishedEvents.AddRange(events);

        return Task.CompletedTask;
    }

    public void Clear(IReadOnlyList<IEvent> events) {
        _toBePublishedEvents.RemoveAll(events.Contains);
    }

    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly List<IEvent> _toBePublishedEvents = []; // TODO: Store locally
}

file class Disposable(Action action) : IDisposable {
    public void Dispose() {
        action();
    }
}
