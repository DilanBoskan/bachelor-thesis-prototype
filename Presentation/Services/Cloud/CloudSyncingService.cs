using Application.Contracts.Services;
using Domain.Aggregates.Books;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Cloud;
public class CloudSyncingService(IReplicationService replicationService, ILogger<CloudSyncingService> logger) {
    private const int DELAY_MS = 1000;
    private readonly IReplicationService _replicationService = replicationService;
    private readonly ILogger<CloudSyncingService> _logger = logger;

    public Task StartAsync(Guid instanceId, BookId bookId, CancellationToken ct) {
        async Task SyncAsync(BookId bookId) {
            _logger.LogInformation("Syncing events for book {BookId}", bookId);

            await _replicationService.PullAsync();
            await _replicationService.PushAsync();
        }

        return Task.Run(async () => {
            while (!ct.IsCancellationRequested) {
                await SyncAsync(bookId);

                await Task.Delay(TimeSpan.FromMilliseconds(DELAY_MS), ct);
            }
        }, ct);
    }
}
