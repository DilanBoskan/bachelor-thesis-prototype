namespace Domain.Aggregates.Pages;
public interface IPageRepository {
    Task<Page?> GetByIdAsync(PageId id, CancellationToken ct = default);
    Task UpdateAsync(Page page, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
