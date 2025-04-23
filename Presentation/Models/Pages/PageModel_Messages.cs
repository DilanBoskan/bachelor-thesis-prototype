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

public sealed partial class PageModel : IRecipient<IPageWindowsEvent> {
    private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;


    private void RegisterMessageHandlers() {
        _messenger.Register<IPageWindowsEvent, PageId>(this, Id);
    }
    private void UnregisterMessagesHandlers() {
        _messenger.UnregisterAll(this);
    }


    public void Receive(IPageWindowsEvent message) {
        switch (message) {
            case WindowsElementCreatedEvent createdMessage:
                Receive(createdMessage);
                break;
            case WindowsElementDeletedEvent deletedMessage:
                Receive(deletedMessage);
                break;
            default:
                throw new NotImplementedException();
        }
    }
    private void Receive(WindowsElementCreatedEvent message) {
        AddElement(message.Element);
    }
    private void Receive(WindowsElementDeletedEvent message) {
        RemoveElement(message.ElementId);
    }
}
