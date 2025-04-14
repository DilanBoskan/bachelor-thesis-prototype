using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.InMemory;

public class InMemoryElementRepository : IElementRepository {
    public IReadOnlyList<Element>? GetFromPage(PageId pageId) {
        ArgumentNullException.ThrowIfNull(pageId);

        if (!_pageElements.ContainsKey(pageId)) {
            // Fill with dummy data
            _pageElements[pageId] = [InkStrokeElement.CreateRandom(), InkStrokeElement.CreateRandom()];
        }

        return _pageElements.TryGetValue(pageId, out var elements) ? elements : null;
    }
    public void CreateInPage(PageId pageId, Element element) {
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(element);

        if (!_pageElements.TryGetValue(pageId, out var elements)) {
            elements = _pageElements[pageId] = [];
        }

        elements.Add(element);
    }
    public void DeleteInPage(PageId pageId, ElementId id) {
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(id);

        if (!_pageElements.TryGetValue(pageId, out var elements))
            return;

        elements.RemoveAll(e => e.Id == id);
    }

    private static readonly Dictionary<PageId, List<Element>> _pageElements = [];
}
