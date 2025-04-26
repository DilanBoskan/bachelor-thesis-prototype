using Application.Contracts.Event;
using Application.Contracts.Services;
using Application.Extensions.Serializer.Events;
using Domain.Aggregates.Pages;
using Domain.Events;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace Application.Features.Replication;

public class ReplicationService(IPageRepository pageRepository, IEventClient eventClient, IMergeService mergeService, EventStorage eventStorage, ILogger<ReplicationService> logger) : IReplicationService {
    private readonly IPageRepository _pageRepository = pageRepository;
    private readonly IEventClient _eventsClient = eventClient;
    private readonly IMergeService _mergeService = mergeService;
    private readonly EventStorage _eventStorage = eventStorage;
    private readonly ILogger<ReplicationService> _logger = logger;

    public async Task<IReadOnlyList<IEvent>> PullAsync(PageId pageId) {
        // Get recent events from server
        if (!_lastRemoteReplication.TryGetValue(pageId, out var lastReplicationId))
            _lastRemoteReplication[pageId] = lastReplicationId = ReplicationId.Empty;

        // Get events
        var page = await _pageRepository.GetByIdAsync(pageId);
        ArgumentNullException.ThrowIfNull(page);

        var pullData = await _eventsClient.PullAsync(pageId, lastReplicationId);

        // Convert to domain events
        var events = Protos.Events.EventGroup.Parser.ParseFrom(pullData.Events).Events
            .Select(EventSerializer.ToDomain)
            .ToList();

        _mergeService.Merge(page, events);

        await _pageRepository.UpdateAsync(page);

        await _pageRepository.SaveChangesAsync();

        _lastRemoteReplication[pageId] = pullData.LatestReplicationId;

        return events.ToList();
    }

    public async Task PushAsync() {
        // Get recent events from client
        var events = _eventStorage.Events.ToList();

        var pageIdGroupedEvents = events
            .GroupBy(x => x.PageId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var (pageId, pageEvents) in pageIdGroupedEvents) {
            if (!_lastRemoteReplication.TryGetValue(pageId, out var lastReplicationId))
                _lastRemoteReplication[pageId] = lastReplicationId = ReplicationId.Empty;

            var protoEvents = pageEvents.Select(EventSerializer.ToProto);
            var eventBytes = new Protos.Events.EventGroup { Events = { protoEvents } }
                .ToByteArray();

            var newReplicationId = await _eventsClient.PushAsync(pageId, lastReplicationId, eventBytes);
            _lastRemoteReplication[pageId] = newReplicationId;
        }
    }

    private readonly Dictionary<PageId, ReplicationId> _lastRemoteReplication = [];
}
