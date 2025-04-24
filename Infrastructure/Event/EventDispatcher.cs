using Application.Contracts.Event;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event;
internal sealed class EventDispatcher(IEnumerable<IEventHandler> eventHandlers, IEnumerable<ICloudEventHandler> cloudEventHandlers) : IEventDispatcher, ICloudEventDispatcher {
    private readonly IReadOnlyCollection<IEventHandler> _eventHandlers = eventHandlers.ToList();
    private readonly IReadOnlyCollection<ICloudEventHandler> _cloudEventHandlers = cloudEventHandlers.ToList();

    async Task IEventDispatcher.PublishAsync(IReadOnlyList<IEvent> events, CancellationToken ct) {
        ArgumentNullException.ThrowIfNull(events);
        if (events.Count == 0) return;

        foreach (var eventHandler in _eventHandlers) {
            await eventHandler.HandleAsync(events);
        }
    }

    async Task ICloudEventDispatcher.PublishAsync(IReadOnlyList<IEvent> events, CancellationToken ct) {
        ArgumentNullException.ThrowIfNull(events);
        if (events.Count == 0) return;

        foreach (var eventHandler in _cloudEventHandlers) {
             await eventHandler.HandleAsync(events);
        }
    }
}
