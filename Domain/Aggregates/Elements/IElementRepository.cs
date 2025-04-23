using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.Elements;

public interface IElementRepository {
    Task<IReadOnlyList<Element>> GetByPageIdAsync(PageId pageId, CancellationToken ct = default);

    Task CreateAsync(Element element, CancellationToken ct = default);
    Task DeleteAsync(ElementId id, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
