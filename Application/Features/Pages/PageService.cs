using Application.Contracts.Command;
using Application.Contracts.Services;
using Application.Features.Pages.Commands;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Domain.Events;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace Application.Features.Pages;
public sealed class PageService(ICommandDispatcher commandDispatcher, IPageRepository pageRepository) : IPageService {
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;
    private readonly IPageRepository _pageRepository = pageRepository;

    public Task<Page?> GetByIdAsync(PageId id, CancellationToken ct = default) => _pageRepository.GetByIdAsync(id, ct);

    public async Task<InkStrokeElement> CreateInkStrokeElementAsync(PageId pageId, DateTime createdAt, IReadOnlyList<InkStrokePoint> points, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(points);

        var inkStroke = await _commandDispatcher.PublishAsync<CreateInkStrokeElementInPageCommand, InkStrokeElement>(new(pageId, createdAt, points), ct);

        return inkStroke;
    }

    public async Task DeleteElementAsync(PageId pageId, ElementId elementId, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(elementId);

        await _commandDispatcher.PublishAsync<RemoveElementFromPageCommand>(new(pageId, elementId), ct);
    }
}
