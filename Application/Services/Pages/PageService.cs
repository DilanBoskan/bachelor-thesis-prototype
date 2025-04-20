using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using Domain.Events;
using Domain.Helpers;
using System.Drawing;
using System.Numerics;

namespace Application.Services.Pages;
public sealed class PageService(IElementRepository elementRepository, IEventPublisher eventPublisher) : IPageService {
    private readonly IElementRepository _elementRepository = elementRepository;
    private readonly IEventPublisher _eventPublisher = eventPublisher;

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
        _eventPublisher.Publish(new ElementCreatedEvent(DateTime.Now, id, element));
    }

    public void DeleteElement(PageId id, ElementId elementId) {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(elementId);

        _elementRepository.DeleteInPage(id, elementId);
        _eventPublisher.Publish(new ElementDeletedEvent(DateTime.Now, id, elementId));
    }
}
