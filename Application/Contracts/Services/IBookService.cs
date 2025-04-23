using Domain.Aggregates.Books;

namespace Application.Contracts.Services;
public interface IBookService {
    Task<Book> GetAsync(BookId id, CancellationToken ct = default);
    Task<BookContent> GetContentAsync(BookId id, CancellationToken ct = default);
}
