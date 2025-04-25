using Application.Contracts.Event;
using Domain.Aggregates.Users;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event;
internal sealed class EventDispatcher(UserId userId, IEnumerable<IEventHandler> eventHandlers) : IEventDispatcher {
    private readonly UserId _userId = userId;
    private readonly IReadOnlyCollection<IEventHandler> _eventHandlers = eventHandlers.ToList();

    async Task IEventDispatcher.PublishAsync(IReadOnlyList<IEvent> events, CancellationToken ct) {
        ArgumentNullException.ThrowIfNull(events);
        if (events.Count == 0) return;

        foreach (var @event in events) {
            @event.UserId = _userId;
        }
        foreach (var eventHandler in _eventHandlers) {
            await eventHandler.HandleAsync(events);
        }
    }
}
