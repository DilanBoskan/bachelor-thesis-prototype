using Domain.Entities.Books;

namespace Application.Services.Books;
public interface IBookService {
    Task<Book> GetAsync(BookId id, CancellationToken ct = default);
    Task<BookContent> GetContentAsync(BookId id, CancellationToken ct = default);
}
