using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Extensions;
using Presentation.Messages.Pages;
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

public sealed partial class PageModel : IRecipient<IPageWindowsMessage> {
    private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;


    private void RegisterMessageHandlers() {
        _messenger.Register<IPageWindowsMessage, PageId>(this, Id);
    }
    private void UnregisterMessagesHandlers() {
        _messenger.UnregisterAll(this);
    }


    public void Receive(IPageWindowsMessage message) {
        switch (message) {
            case WindowsElementCreatedMessage createdMessage:
                Receive(createdMessage);
                break;
            case WindowsElementDeletedMessage deletedMessage:
                Receive(deletedMessage);
                break;
            default:
                throw new NotImplementedException();
        }
    }
    private void Receive(WindowsElementCreatedMessage message) {
        AddElement(message.Element);
    }
    private void Receive(WindowsElementDeletedMessage message) {
        RemoveElement(message.ElementId);
    }
}
