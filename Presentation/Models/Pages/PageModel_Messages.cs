using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Domain.Aggregates.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Events.Pages;
using Presentation.Extensions;
using Presentation.Services.Pages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Page;

public sealed partial class PageModel : IRecipient<WindowsElementCreatedEvent>, IRecipient<WindowsElementDeletedEvent> {
    private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

    private void RegisterMessageHandlers() {
        _messenger.Register<WindowsElementCreatedEvent, PageId>(this, Id);
        _messenger.Register<WindowsElementDeletedEvent, PageId>(this, Id);
    }
    private void UnregisterMessagesHandlers() {
        _messenger.UnregisterAll(this);
    }

    public void Receive(WindowsElementCreatedEvent message) {
        AddElement(message.Element);
    }
    public void Receive(WindowsElementDeletedEvent message) {
        RemoveElement(message.ElementId);
    }
}
