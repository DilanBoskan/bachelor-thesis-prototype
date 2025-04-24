using Application.Contracts.Event;
using Domain.Aggregates;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Repositories.InMemory;

public class InMemoryElementRepository(UserId userId, IEventDispatcher eventDispatcher) : IElementRepository {
    private readonly UserId _userId = userId;
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

    public Task<IReadOnlyList<Element>> GetByPageIdAsync(PageId pageId, CancellationToken ct = default) {
        return _pageElements.TryGetValue(pageId, out var elements)
            ? Task.FromResult<IReadOnlyList<Element>>(elements)
            : Task.FromResult<IReadOnlyList<Element>>(Array.Empty<Element>());
    }

    public Task CreateAsync(Element element, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(element);

        if (!_pageElements.TryGetValue(element.PageId, out var elements)) {
            elements = _pageElements[element.PageId] = [];
        }

        elements.Add(element);
        _modifiedElements.Add(element);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(ElementId id, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(id);

        foreach (var pageId in _pageElements.Keys) {
            var element = _pageElements[pageId].Find(e => e.Id == id);
            if (element is not null) {
                _deletedElements.Add(element);
                _pageElements[pageId].Remove(element);
                return Task.CompletedTask;
            }
        }

        throw new Exception($"Element with Id {id} not found.");
    }

    public async Task SaveChangesAsync(CancellationToken ct = default) {
        var events = _modifiedElements
            .SelectMany(e => e.PopDomainEvents())
            .Concat(_deletedElements.Select(e => new ElementDeletedEvent(e.BookId, e.PageId, e.Id)))
            .ToList();

        // Save changes (EF Core)

        foreach (var @event in events) {
            @event.UserId = _userId;
        }
        await _eventDispatcher.PublishAsync(events, CancellationToken.None);

        _modifiedElements.Clear();
        _deletedElements.Clear();
    }

    private readonly List<Element> _modifiedElements = [];
    private readonly List<Element> _deletedElements = [];

    private static readonly Dictionary<PageId, List<Element>> _pageElements = [];
}
