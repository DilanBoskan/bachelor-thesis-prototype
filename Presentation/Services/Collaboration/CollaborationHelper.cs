using CommunityToolkit.Mvvm.Messaging;
using Domain.Aggregates.Books;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Events;
using Presentation.Events.Pages;
using Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Presentation.Services.Collaboration;

public sealed class CollaborationHelper {
    public const int DELAY_MS = 5000; // 5 seconds

    private readonly IEventManagerFactory _eventManagerFactory = App.Current.ServiceProvider.GetRequiredService<IEventManagerFactory>();
    private readonly ILogger<CollaborationHelper> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<CollaborationHelper>>();

    public Task StartAsync(BookId bookId, CancellationToken ct) {
        var eventManager = _eventManagerFactory.Create(bookId);

        async Task FlushAndGetEventsAsync(BookId bookId) {
            await eventManager.FlushAsync();

            var events = await eventManager.GetEventsAsync();
            if (!events.Any()) {
                _logger.LogDebug("No events");
                return;
            }

            _logger.LogDebug("Received {Count} events", events.Count);
            PublishEvents(events);
        }

        return Task.Run(async () => {
            while (!ct.IsCancellationRequested) {
                await FlushAndGetEventsAsync(bookId);

                await Task.Delay(TimeSpan.FromMilliseconds(DELAY_MS), ct);
            }
        }, ct);
    }

    private static void PublishEvents(IReadOnlyList<Event> events) {
        var messenger = WeakReferenceMessenger.Default;

        foreach (var @event in events) {
            switch (@event.ToWindows()) {
                case IPageWindowsEvent pageEvent:
                    messenger.Send(pageEvent, pageEvent.PageId);
                    break;
                case IWindowsEvent windowsEvent:
                    messenger.Send(windowsEvent);
                    break;
            }
        }
    }
}
