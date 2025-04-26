using Application.Contracts.Services;
using Domain.Aggregates.Books;

namespace Application.Features.Books;
public sealed class BookService(IBookRepository bookRepository) : IBookService {
    public Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default) => bookRepository.GetByIdAsync(id, ct);
}
