using Application.Contracts.Cloud;
using Application.Contracts.Event;
using Application.Extensions.Serializer.Events;
using Domain.Aggregates.Books;
using Domain.Events;
using Google.Protobuf;
using Infrastructure.Event;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Cloud;

public class CloudManager(Guid instanceId, BookId bookId, EventAggregator eventAggregator, IEventsClient eventsClient, ICloudEventDispatcher cloudEventDispatcher, ILogger<CloudManager> logger) : ICloudManager {
    private readonly EventAggregator _eventAggregator = eventAggregator;
    private readonly IEventsClient _eventsClient = eventsClient;
    private readonly ICloudEventDispatcher _cloudEventDispatcher = cloudEventDispatcher;

    public Guid InstanceId { get; } = instanceId;
    public BookId BookId { get; } = bookId;

    public async Task PullAsync() {
        // Get recent events from server
        var newLastUpdated = DateTime.UtcNow;
        var eventBytes = await _eventsClient.PullAsync(BookId, InstanceId, _lastUpdated);

        var events = Application.Protos.Events.EventGroup.Parser
            .ParseFrom(eventBytes)
            .Events
            .Select(EventSerializer.ToDomain)
            .ToList();

        // Publish to internal handlers
        await _cloudEventDispatcher.PublishAsync(events);

        _lastUpdated = newLastUpdated;
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

    private DateTime _lastUpdated = DateTime.MinValue.ToUniversalTime(); // TODO: Get last updated time from local storage
}
