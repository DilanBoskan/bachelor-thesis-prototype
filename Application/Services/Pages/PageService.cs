using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using Domain.Helpers;
using Domain.Messages;
using System.Drawing;
using System.Numerics;

namespace Application.Services.Pages;
public sealed class PageService(IElementRepository elementRepository, IMessagePublisher messagePublisher) : IPageService {
    private readonly IElementRepository _elementRepository = elementRepository;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;

    public Page Get(PageId id) => new(id, new SizeF(1000, 1414));
    public PageContent GetContent(PageId id) {
        ArgumentNullException.ThrowIfNull(id);

        var elements = _elementRepository.GetFromPage(id);
        ArgumentNullException.ThrowIfNull(elements);

        return new PageContent(elements);
    }

    public void CreateElement<T>(PageId id, T element) where T : Element {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(element);

        _elementRepository.CreateInPage(id, element);
        _messagePublisher.Publish(new ElementCreatedMessage(DateTime.Now, id, element));
    }

    public void DeleteElement(PageId id, ElementId elementId) {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(elementId);

        _elementRepository.DeleteInPage(id, elementId);
        _messagePublisher.Publish(new ElementDeletedMessage(DateTime.Now, id, elementId));
    }
}
