using Application.Contracts.Services;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;

namespace Application.Features.Pages;
public sealed class PageService(IPageRepository pageRepository) : IPageService {
    private readonly IPageRepository _pageRepository = pageRepository;

    public Task<Page?> GetByIdAsync(PageId id, CancellationToken ct = default) => _pageRepository.GetByIdAsync(id, ct);

    public async Task<InkStrokeElement> CreateInkStrokeElementAsync(PageId pageId, DateTime createdAt, IReadOnlyList<InkStrokePoint> points, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(points);

        var page = await _pageRepository.GetByIdAsync(pageId, ct);
        ArgumentNullException.ThrowIfNull(page);

        var element = page.CreateInkStrokeElement(createdAt, points);

        await _pageRepository.UpdateAsync(page, ct);

        await _pageRepository.SaveChangesAsync(ct);

        return element;
    }

    public async Task DeleteElementAsync(PageId pageId, ElementId elementId, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(elementId);

        var page = await _pageRepository.GetByIdAsync(pageId, ct);
        ArgumentNullException.ThrowIfNull(page);

        page.RemoveElement(elementId);

        await _pageRepository.UpdateAsync(page, ct);

        await _pageRepository.SaveChangesAsync(ct);
    }
}
