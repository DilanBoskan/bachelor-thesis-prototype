using Application.Contracts.Cloud;
using Domain.Aggregates.Books;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Cloud;
public class CloudSyncingService(ICloudManagerFactory cloudManagerFactory, ILogger<CloudSyncingService> logger) {
    private const int DELAY_MS = 1000;
    private readonly ICloudManagerFactory _cloudManagerFactory = cloudManagerFactory;
    private readonly ILogger<CloudSyncingService> _logger = logger;

    public Task StartAsync(Guid instanceId, BookId bookId, CancellationToken ct) {
        var cloudManager = _cloudManagerFactory.Create(instanceId, bookId);

        async Task SyncAsync(BookId bookId) {
            _logger.LogInformation("Syncing events for book {BookId}", bookId);

            await cloudManager.PullAsync();
            await cloudManager.PushAsync();
        }

        return Task.Run(async () => {
            while (!ct.IsCancellationRequested) {
                await SyncAsync(bookId);

                await Task.Delay(TimeSpan.FromMilliseconds(DELAY_MS), ct);
            }
        }, ct);
    }
}
