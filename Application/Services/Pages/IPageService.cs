using Domain.Entities.Pages;

namespace Application.Services.Pages;

public interface IPageService {
    Task<Page> GetAsync(PageId id, CancellationToken ct = default);
    Task<PageContent> GetContentAsync(PageId id, CancellationToken ct = default);
}
