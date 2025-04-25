using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.Pages;
public interface IPageRepository {
    Task<Page?> GetByIdAsync(PageId id, CancellationToken ct = default);
    Task UpdateAsync(Page page, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
