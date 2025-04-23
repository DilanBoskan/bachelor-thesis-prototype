using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;

namespace Application.Contracts.Services;

public interface IPageService {
    Task<Page> GetAsync(PageId id);
    Task<PageContent> GetContentAsync(PageId id);
}
