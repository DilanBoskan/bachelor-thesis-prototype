
using Domain.Entities.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using Domain.Events;

namespace WebApi.EndpointDefinitions;

public class EventsEndpoint : IEndpointDefinition {
    public void MapDefinitions(WebApplication app) {
        var group = app.MapGroup("events");

        group.MapGet("/{bookId}", GetMessage);
        group.MapPost("/{bookId}", PostMessage);
    }

    [Produces("application/x-protobuf")]
    private static byte[] GetMessage([FromRoute] BookId bookId, [FromQuery] Guid userId, [FromQuery] DateTime? from = null) {
        from ??= DateTime.MinValue; // Default to the earliest date if not provided

        var eventGroup = new Domain.Protos.Events.EventGroup();
        lock (_queueLock) {
            if (_publishedEvents.ContainsKey(bookId)) {
                eventGroup.Events.AddRange(_publishedEvents[bookId]
                    .Where(e => e.Key > from && e.Value.UserId != userId) // Not self-published events
                    .SelectMany(e => e.Value.Events)
                    .Select(Event.ToProto));
            }
        }

        return eventGroup.ToByteArray();
    }

    private static void PostMessage(BookId bookId, [FromQuery] Guid userId, [FromBody] byte[] data) {
        var eventGroup = Domain.Protos.Events.EventGroup.Parser.ParseFrom(data);
        var events = eventGroup.Events.Select(Event.FromProto).ToArray();

        lock (_queueLock) {
            if (!_publishedEvents.ContainsKey(bookId))
                _publishedEvents[bookId] = new();

            _publishedEvents[bookId].Add(DateTime.Now, (userId, events));
        }
    }


    private static readonly Lock _queueLock = new();
    private static readonly Dictionary<BookId, SortedList<DateTime, (Guid UserId, Event[] Events)>> _publishedEvents = [];
}
