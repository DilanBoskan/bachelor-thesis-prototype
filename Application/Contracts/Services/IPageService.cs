using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;

namespace Application.Contracts.Services;

public interface IPageService {
    Task<Page?> GetByIdAsync(PageId id, CancellationToken ct = default);

    Task<InkStrokeElement> CreateInkStrokeElementAsync(PageId pageId, DateTime createdAt, IReadOnlyList<InkStrokePoint> points, CancellationToken ct = default);
    Task DeleteElementAsync(PageId pageId, ElementId elementId, CancellationToken ct = default);
}
