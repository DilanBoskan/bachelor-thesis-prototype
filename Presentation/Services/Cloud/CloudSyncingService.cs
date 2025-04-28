using Application.Contracts.Services;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using Domain.Events;
using Microsoft.Extensions.Logging;
using Presentation.Messenges;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Cloud;
public class CloudSyncingService(IReplicationService replicationService, ILogger<CloudSyncingService> logger) {
    private const int DELAY_MS = 1000;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
    private readonly IReplicationService _replicationService = replicationService;
    private readonly ILogger<CloudSyncingService> _logger = logger;

    public Task StartAsync(CancellationToken ct) {
        async Task SyncAsync() {
            foreach (var pageId in GetLoadedPages()) {
                var events = await _replicationService.PullAsync(pageId);
                _messenger.Send(events, pageId);
            }
            await _replicationService.PushAsync();
        }

        return Task.Run(async () => {
            while (!ct.IsCancellationRequested) {
                try {
                    await SyncAsync();
                } catch (Exception e) {
                    _logger.LogError(e, "Syncing failed!");
                }

                await Task.Delay(TimeSpan.FromMilliseconds(DELAY_MS), ct);
            }
        }, ct);
    }

    private IReadOnlyCollection<PageId> GetLoadedPages() {
        var loadedPageIdsMessage = _messenger.Send<RequestLoadedPagesMessage>();
        if (!loadedPageIdsMessage.HasReceivedResponse)
            return Array.Empty<PageId>();

        return loadedPageIdsMessage.Response;
    }
}
