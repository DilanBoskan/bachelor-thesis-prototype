using Application.Contracts.Command;
using Application.Contracts.Services;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Domain.Events;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace Application.Features.Pages;
public sealed class PageService(IElementRepository elementRepository) : IPageService {
    private readonly IElementRepository _elementRepository = elementRepository;

    public Task<Page> GetAsync(PageId id) => Task.FromResult<Page>(new(id, BookId.Create(Guid.Empty), new SizeF(1000, 1414)));
    public async Task<PageContent> GetContentAsync(PageId id) {
        ArgumentNullException.ThrowIfNull(id);

        var elements = await _elementRepository.GetByPageIdAsync(id);

        return new PageContent(elements);
    }
}
