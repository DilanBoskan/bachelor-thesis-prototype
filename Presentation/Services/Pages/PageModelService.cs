using Application.Contracts.Services;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
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
public class PageModelService(IPageService pageService, IElementService elementService) : IPageModelService {
    private readonly IPageService _pageService = pageService;
    private readonly IElementService _elementService = elementService;

    public async Task<InkStrokeElementModel> CreateInkStrokeElementAsync(BookId bookId, PageId id, DateTime createdAt, InkStrokePoint[] points, CancellationToken ct = default) {
        var inkStroke = await App.Current.DatabaseScheduler.EnqueueAsync(async () => {
            var inkStroke = await _elementService.CreateInkStrokeElementAsync(bookId, id, createdAt, points);
            return inkStroke;
        }, ct);

        return inkStroke.ToWindows();
    }
    public async Task<PageModelContent> GetContentAsync(PageId id, CancellationToken ct = default) {
        var page = await Task.Run(async () => await _pageService.GetContentAsync(id), ct);

        var elements = page.Elements
            .Select(e => e.ToWindows())
            .ToArray();

        return new PageModelContent(elements);
    }
}
