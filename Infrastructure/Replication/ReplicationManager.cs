using Application.Contracts.Event;
using Application.Contracts.Replication;
using Application.Extensions.Serializer.Events;
using Domain.Aggregates.Books;
using Domain.Events;
using Google.Protobuf;
using Infrastructure.Event;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Replication;

public class ReplicationManager(Guid instanceId, BookId bookId, EventAggregator eventAggregator, IEventsClient eventsClient, ILogger<ReplicationManager> logger) : IReplicationManager {
    private readonly EventAggregator _eventAggregator = eventAggregator;
    private readonly IEventsClient _eventsClient = eventsClient;
    private readonly ILogger<ReplicationManager> _logger = logger;

    public Guid InstanceId { get; } = instanceId;
    public BookId BookId { get; } = bookId;

    public async Task PullAsync() {
        // Get recent events from server
        var newLastUpdated = DateTime.UtcNow;
        var eventBytes = await _eventsClient.PullAsync(BookId, InstanceId, _lastReplicaVersion);

        var events = Application.Protos.Events.EventGroup.Parser
            .ParseFrom(eventBytes)
            .Events
            .Select(EventSerializer.ToDomain)
            .ToList();

        _lastUpdated = newLastUpdated;

        return events;
    }

    public async Task PushAsync() {
        if (_eventAggregator.Events.Count == 0) return;
        // Get recent events from client
        var events = _eventAggregator.Events.ToList();
        var protoEvents = events.Select(EventSerializer.ToProto);
        var eventBytes = new Application.Protos.Events.EventGroup { Events = { protoEvents } }
            .ToByteArray();

        await _eventsClient.PushAsync(BookId, InstanceId, eventBytes);

        _eventAggregator.Clear(events);
    }

    private ReplicationVersion _lastReplication = Application.Contracts.Replication.ReplicationVersion.Empty; // TODO: Get last replication from local storage
}
