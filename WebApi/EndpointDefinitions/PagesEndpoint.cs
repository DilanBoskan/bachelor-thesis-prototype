using Microsoft.AspNetCore.Mvc;
using Google.Protobuf;
using Application.Extensions.Serializer.Events;
using Microsoft.AspNetCore.Http.HttpResults;
using Application.Contracts.Event;
using Domain.Aggregates.Pages;
using Application.Extensions.Serializer.Pages;

namespace WebApi.EndpointDefinitions;

public class PagesEndpoint : IEndpointDefinition {
    public void MapDefinitions(WebApplication app) {
        var group = app.MapGroup("books");

        group.MapGet("pages/{pageId}/_replication", PullAsync);
        group.MapPatch("pages/{pageId}/_replication", PatchAsync);
        group.MapPost("pages/{pageId}/_replication", ForcePushAsync);
    }


    public static async Task<Results<Ok<PullData>, BadRequest<string>>> PullAsync(PageId pageId, [FromHeader(Name = "X-Replication-Id")] ReplicationId fromReplicationId) {
        ReplicationId latestReplicationId;
        byte[] eventBytes;
        lock (_queueLock) {
            if (!_pages.TryGetValue(pageId, out var page))
                return TypedResults.BadRequest("Nothing to pull!");

            var events = page.DomainEvents.Where(e => e.ReplicationId > fromReplicationId).ToList();
            var eventGroup = new Application.Protos.Events.EventGroup() {
                Events = { events.Select(EventSerializer.ToProto) }
            };

            latestReplicationId = page.ReplicationId;
            eventBytes = eventGroup.ToByteArray();
        }

        return TypedResults.Ok(new PullData(pageId, latestReplicationId, fromReplicationId, eventBytes));
    }

    public static async Task<Results<Ok<ReplicationId>, BadRequest<string>, Conflict<string>>> PatchAsync(PageId pageId, [FromHeader(Name = "X-Replication-Id")] ReplicationId latestReplicationId, [FromBody] byte[] data) {
        var eventGroup = Application.Protos.Events.EventGroup.Parser.ParseFrom(data);
        var events = eventGroup.Events.Select(EventSerializer.ToDomain).ToArray();
        if (events.Length == 0)
            return TypedResults.BadRequest("No message specified");

        ReplicationId newReplicationId;
        lock (_queueLock) {
            if (!_pages.TryGetValue(pageId, out var page))
                return TypedResults.BadRequest("Nothing to patch!");

            if (page.ReplicationId != latestReplicationId)
                return TypedResults.Conflict("Replication Ids do not match!");

            foreach (var @event in events) {
                page.Apply(@event);
            }

            newReplicationId = page.ReplicationId;
        }

        return TypedResults.Ok(newReplicationId);
    }
    public static async Task<Ok> ForcePushAsync(PageId pageId, [FromBody] byte[] data) {
        var page = PageSerializer.ToDomain(Application.Protos.Pages.Page.Parser.ParseFrom(data));

        lock (_queueLock) {
            _pages[page.Id] = page;
        }

        return TypedResults.Ok();
    }


    private static readonly Lock _queueLock = new();
    private static readonly Dictionary<PageId, Page> _pages = [];
}
