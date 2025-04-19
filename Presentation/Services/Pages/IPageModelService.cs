using Domain.Entities.Elements;
using Domain.Entities.Pages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Pages;
public interface IPageModelService {
    Task<PageModelContent> GetContentAsync(PageId id, CancellationToken ct = default);
    Task CreateElementsAsync(PageId id, IReadOnlyList<Element> elements, CancellationToken ct = default);
    Task CreateElementAsync(PageId id, Element element, CancellationToken ct = default);
}
