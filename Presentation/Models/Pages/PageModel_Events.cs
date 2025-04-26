using CommunityToolkit.Mvvm.Messaging;
using Domain.Aggregates.Pages;
using Domain.Events;
using Presentation.Models.Elements.InkStrokes;

namespace Presentation.Models.Pages;

public sealed partial class PageModel : IRecipient<IEvent> {
    private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

    private void RegisterMessageHandlers() {
        _messenger.Register<IEvent, PageId>(this, Id);
    }
    private void UnregisterMessagesHandlers() {
        _messenger.UnregisterAll(this);
    }

    public void Receive(IEvent eventObj) {
        switch (eventObj) {
            case InkStrokeElementAddedToPageEvent @event:
                Receive(@event);
                break;
            case ElementRemovedFromPageEvent @event:
                Receive(@event);
                break;
        }
    }

    public void Receive(InkStrokeElementAddedToPageEvent @event) {
        var inkStrokeModel = new InkStrokeElementModel(@event.ToInkStrokeElement());
        AddElement(inkStrokeModel);
    }
    public void Receive(ElementRemovedFromPageEvent @event) {
        RemoveElement(@event.ElementId);
    }
}
