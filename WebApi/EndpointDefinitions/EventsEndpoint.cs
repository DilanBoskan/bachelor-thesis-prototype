using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using Domain.Events;
using Domain.Aggregates.Books;
using Application.Extensions.Serializer.Events;

namespace WebApi.EndpointDefinitions;

public class EventsEndpoint : IEndpointDefinition {
    public void MapDefinitions(WebApplication app) {
        var group = app.MapGroup("events");

        group.MapGet("/{bookId}", GetMessage);
        group.MapPost("/{bookId}", PostEvent);
    }

    [Produces("application/x-protobuf")]
    private static byte[] GetMessage([FromRoute] BookId bookId, [FromQuery] Guid instanceId, [FromQuery] DateTime? from = null) {
        from ??= DateTime.MinValue; // Default to the earliest date if not provided

        var eventGroup = new Application.Protos.Events.EventGroup();
        lock (_queueLock) {
            if (_publishedEvents.ContainsKey(bookId)) {
                eventGroup.Events.AddRange(_publishedEvents[bookId]
                    .Where(e => e.Key > from && e.Value.InstanceId != instanceId) // Not self-published events
                    .SelectMany(e => e.Value.Events)
                    .Select(EventSerializer.ToProto));
            }
        }

        return eventGroup.ToByteArray();
    }

    private static void PostEvent(BookId bookId, [FromQuery] Guid instanceId, [FromBody] byte[] data) {
        var eventGroup = Application.Protos.Events.EventGroup.Parser.ParseFrom(data);
        var events = eventGroup.Events.Select(EventSerializer.ToDomain).ToArray();
        if (events.Length == 0)
            return;

        lock (_queueLock) {
            if (!_publishedEvents.ContainsKey(bookId))
                _publishedEvents[bookId] = new();

            _publishedEvents[bookId].Add(DateTime.UtcNow, (instanceId, events));
        }
    }


    private static readonly Lock _queueLock = new();
    private static readonly Dictionary<BookId, SortedList<DateTime, (Guid InstanceId, IEvent[] Events)>> _publishedEvents = [];
}
