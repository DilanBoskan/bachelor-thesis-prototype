using Domain.Entities.Books;
using Domain.Events;
using Google.Protobuf;
using Infrastructure.Events.Local;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Messages.Local;
public partial class EventDispatcher(Guid userId, IEventsClient eventsClient) : IEventPublisher {
    private readonly IEventsClient _eventsClient = eventsClient;

    private readonly Lock _queueLock = new();
    public void Publish(Event @event) {
        lock (_queueLock) {
            _preparedEvents.Add(@event);
        }
    }

    public async Task FlushAsync(BookId bookId) {
        Event[] events;
        lock (_queueLock) {
            if (_preparedEvents.Count == 0)
                return;

            events = _preparedEvents.ToArray();
            _preparedEvents.Clear();
        }

        var eventGroup = new Domain.Protos.Events.EventGroup();
        eventGroup.Events.AddRange(events.Select(Event.ToProto));

        await _eventsClient.PostEventsAsync(bookId, userId, eventGroup.ToByteArray());
    }

    private readonly List<Event> _preparedEvents = [];
}
