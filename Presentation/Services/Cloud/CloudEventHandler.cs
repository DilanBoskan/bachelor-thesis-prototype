using Application.Contracts.Event;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Domain.Aggregates.Pages;
using Domain.Events;
using Microsoft.Extensions.Logging;
using Presentation.Events;
using Presentation.Events.Pages;
using Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace Presentation.Services.Cloud;

public class CloudEventHandler(ILogger<CloudEventHandler> logger) : ICloudEventHandler {
    private readonly ILogger _logger = logger;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

    public async Task HandleAsync(IReadOnlyList<IEvent> events) {
        await App.Current.UIScheduler.EnqueueAsync(() => {
            foreach (var @event in events) {
                switch (@event) {
                    case ElementCreatedEvent elementCreatedEvent:
                        Receive(elementCreatedEvent);
                        break;
                    case ElementDeletedEvent elementDeletedEvent:
                        Receive(elementDeletedEvent);
                        break;
                }
            }
        });
    }

    public void Receive(ElementCreatedEvent @event) {
        _logger.LogDebug("Element created: {ElementId}", @event.Element.Id);
        _messenger.Send(@event.ToWindows(), @event.Element.PageId);
    }
    public void Receive(ElementDeletedEvent @event) {
        _logger.LogDebug("Element deleted: {ElementId}", @event.ElementId);
        _messenger.Send(@event.ToWindows(), @event.PageId);
    }
}
