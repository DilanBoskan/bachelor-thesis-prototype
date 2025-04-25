using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using Domain.Events;
using Domain.Aggregates.Books;
using Application.Extensions.Serializer.Events;
using System.IO.Pipes;
using Application.Contracts.Replication;
using Microsoft.AspNetCore.Http.HttpResults;
using Infrastructure.Event;

namespace WebApi.EndpointDefinitions;

public class BooksEndpoint : IEndpointDefinition {
    public void MapDefinitions(WebApplication app) {
        var group = app.MapGroup("books");

        group.MapGet("/{bookId}/events", Pull);
        group.MapPost("/{bookId}/events", Push);
    }

    [Produces("application/x-protobuf")]
    private static Results<Ok<PullData>, BadRequest<string>> Pull([FromRoute] BookId bookId, [FromHeader(Name = "X-Replication-Id")] ReplicationVersion fromReplicationId) {
        ReplicationVersion latestReplicationId;
        IReadOnlyList<(ReplicationVersion, byte[])> events;
        lock (_queueLock) {
            if (!_publishedEvents.TryGetValue(bookId, out var value))
                return TypedResults.BadRequest("Nothing to pull!");

            (latestReplicationId, var history) = value;

            var eventGroup = new Application.Protos.Events.EventGroup();
            events = history
                .Where(h => h.ReplicationId > fromReplicationId)
                .Select(h => (h.ReplicationId, new Application.Protos.Events.EventGroup() {
                    Events = { h.Events.Select(EventSerializer.ToProto) }
                }.ToByteArray()))
                .ToList();
        }

        return TypedResults.Ok(new PullData(latestReplicationId, fromReplicationId, events));
    }

    private static Results<Ok<ReplicationVersion>, BadRequest<string>, Conflict<string>> Push(BookId bookId, [FromHeader(Name = "X-Replication-Id")] ReplicationVersion latestReplicationId, [FromBody] byte[] data) {
        var eventGroup = Application.Protos.Events.EventGroup.Parser.ParseFrom(data);
        var events = eventGroup.Events.Select(EventSerializer.ToDomain).ToArray();
        if (events.Length == 0)
            return TypedResults.BadRequest("No message specified");

        ReplicationVersion newReplicationId;
        lock (_queueLock) {
            if (!_publishedEvents.ContainsKey(bookId))
                _publishedEvents[bookId] = (ReplicationVersion.New(), []);

            var (replicationId, history) = _publishedEvents[bookId];
            if (replicationId != latestReplicationId)
                return TypedResults.Conflict("Replication Ids do not match!");

            newReplicationId = replicationId.Next();
            history.Add((newReplicationId, events));
        }

        return TypedResults.Ok(newReplicationId);
    }


    private static readonly Lock _queueLock = new();
    private static readonly Dictionary<BookId, (ReplicationVersion ReplicationId, List<(ReplicationVersion ReplicationId, IEvent[] Events)> History)> _publishedEvents = [];
}
