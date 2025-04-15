using Application.Services.Pages;
using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using Presentation.Extensions;
using Presentation.Models.Elements.InkStrokes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace Presentation.Services.Pages;
public class WindowsPageService(IPageService pageService) : IWindowsPageService {
    private readonly IPageService _pageService = pageService;

    public async Task CreateElementsAsync(PageId id, IReadOnlyList<Element> elements, CancellationToken ct = default) {
        await Task.Run(() => {
            foreach (var element in elements) {
                _pageService.CreateElement(id, element);
            }
        }, ct);
    }
    public async Task CreateElementAsync(PageId id, Element element, CancellationToken ct = default) {
        await Task.Run(() => _pageService.CreateElement(id, element), ct);
    }
    public async Task<WindowsPageContent> GetContentAsync(PageId id, CancellationToken ct = default) {
        var page = await Task.Run(() => _pageService.GetContent(id), ct);

        var elements = page.Elements
            .Select(e => e.ToWindows())
            .ToArray();

        return new WindowsPageContent(elements);
    }
}
