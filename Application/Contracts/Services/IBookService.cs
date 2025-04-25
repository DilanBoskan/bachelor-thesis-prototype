using Domain.Aggregates.Books;

namespace Application.Contracts.Services;
public interface IBookService {
    Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default);
}
