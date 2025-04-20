
using Domain.Entities.Books;
using Domain.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Protobuf;

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

        var messageGroup = new Domain.Protos.Messages.MessageGroup();
        lock (_queueLock) {
            if (_publishedMessages.ContainsKey(bookId)) {
                messageGroup.Messages.AddRange(_publishedMessages[bookId]
                    .Where(e => e.Key > from && e.Value.UserId != userId) // Not self-published events
                    .SelectMany(e => e.Value.Messages)
                    .Select(Message.ToProto));
            }
        }

        return messageGroup.ToByteArray();
    }

    private static void PostMessage(BookId bookId, [FromQuery] Guid userId, [FromBody] byte[] data) {
        var messageGroup = Domain.Protos.Messages.MessageGroup.Parser.ParseFrom(data);
        var messages = messageGroup.Messages.Select(Message.FromProto).ToArray();

        lock (_queueLock) {
            if (!_publishedMessages.ContainsKey(bookId))
                _publishedMessages[bookId] = new();

            _publishedMessages[bookId].Add(DateTime.Now, (userId, messages));
        }
    }


    private static readonly Lock _queueLock = new();
    private static readonly Dictionary<BookId, SortedList<DateTime, (Guid UserId, Message[] Messages)>> _publishedMessages = [];
}
