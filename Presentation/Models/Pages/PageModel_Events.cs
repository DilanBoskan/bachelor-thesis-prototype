using CommunityToolkit.Mvvm.Messaging;
using Domain.Aggregates.Pages;
using Domain.Events;
using Presentation.Messenges;
using Presentation.Models.Elements.InkStrokes;
using System.Collections.Generic;

namespace Presentation.Models.Pages;

public sealed partial class PageModel : IRecipient<IReadOnlyList<Event>>, IRecipient<RequestLoadedPagesMessage> {
    private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

    private void RegisterMessageHandlers() {
        _messenger.Register<IReadOnlyList<Event>, PageId>(this, Id);
        _messenger.Register<RequestLoadedPagesMessage>(this);
    }
    private void UnregisterMessagesHandlers() {
        _messenger.UnregisterAll(this);
    }

    public void Receive(IReadOnlyList<Event> events) {
        foreach (var eventObj in events) {
            switch (eventObj) {
                case InkStrokeElementAddedToPageEvent @event:
                    Receive(@event);
                    break;
                case ElementRemovedFromPageEvent @event:
                    Receive(@event);
                    break;
            }
        }
    }

    public void Receive(InkStrokeElementAddedToPageEvent @event) {
        var inkStrokeModel = new InkStrokeElementModel(@event.ToInkStrokeElement());
        AddElement(inkStrokeModel);
    }
    public void Receive(ElementRemovedFromPageEvent @event) {
        RemoveElement(@event.ElementId);
    }

    public void Receive(RequestLoadedPagesMessage message) {
        if (message.HasReceivedResponse) {
            message.Response.Add(Id);
        } else {
            message.Reply(new() { Id });
        }
    }
}
