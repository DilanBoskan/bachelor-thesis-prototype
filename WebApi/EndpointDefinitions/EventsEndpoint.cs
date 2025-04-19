
using Domain.Entities.Books;
using Domain.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.EndpointDefinitions;

public class EventsEndpoint : IEndpointDefinition {
    public void MapDefinitions(WebApplication app) {
        var group = app.MapGroup("events");

        group.MapGet("/{bookId}", GetMessage);
        group.MapPost("/{bookId}", PostMessage);
    }

    private static IReadOnlyList<Message> GetMessage(Guid bookId, [FromQuery(Name = "userId")] Guid userId, [FromQuery(Name = "from")] DateTime? from = null) {
        from ??= DateTime.MinValue; // Default to the earliest date if not provided
        var bId = BookId.Create(bookId);

        List<Message> events;
        lock (_queueLock) {
            if (!_publishedMessages.ContainsKey(bId))
                return Array.Empty<Message>();

            events = _publishedMessages[bId]
                .Where(e => e.Key > from && e.Value.UserId != userId) // Not self-published events
                .SelectMany(e => e.Value.Messages)
                .ToList();
        }

        return events;
    }
    private static void PostMessage(Guid bookId, [FromQuery(Name = "userId")] Guid userId, [FromBody] Message[] messages) {
        var bId = BookId.Create(bookId);

        lock (_queueLock) {
            if (!_publishedMessages.ContainsKey(bId))
                _publishedMessages[bId] = new();

            _publishedMessages[bId].Add(DateTime.Now, (userId, messages));
        }
    }


    private static readonly Lock _queueLock = new();
    private static readonly Dictionary<BookId, SortedList<DateTime, (Guid UserId, Message[] Messages)>> _publishedMessages = [];
}
