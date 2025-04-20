using Application.Services.Messages;
using Application.Services.Pages;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Entities.Books;
using Domain.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Extensions;
using Presentation.Messages;
using Presentation.Messages.Pages;
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

    private readonly IMessageManagerFactory _messageManagerFactory = App.Current.ServiceProvider.GetRequiredService<IMessageManagerFactory>();
    private readonly ILogger<CollaborationHelper> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<CollaborationHelper>>();

    public Task StartAsync(BookId bookId, CancellationToken ct) {
        var messageManager = _messageManagerFactory.Create(bookId);

        async Task FlushAndGetEventsAsync(BookId bookId) {
            await messageManager.FlushAsync();

            var events = await messageManager.GetEventsAsync();
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

    private static void PublishEvents(IReadOnlyList<Message> messages) {
        var messenger = WeakReferenceMessenger.Default;

        foreach (var message in messages) {
            switch (message.ToWindows()) {
                case IPageWindowsMessage pageMessage:
                    messenger.Send(pageMessage, pageMessage.PageId);
                    break;
                case IWindowsMessage windowsMessage:
                    messenger.Send(windowsMessage);
                    break;
            }
        }
    }
}
