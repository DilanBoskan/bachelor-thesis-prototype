using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using System.Drawing;

namespace Infrastructure.Persistance.Repositories.InMemory;
public class InMemoryBookRepository : IBookRepository {
    public Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default) => Task.FromResult(_books.TryGetValue(id, out var page) ? page : null);


    private static readonly Dictionary<BookId, Book> _books = new() {
       { BookId.Create(Guid.Empty), new Book(BookId.Create(Guid.Empty), DateTime.UtcNow, DateTime.UtcNow, [
            new Book.Page(PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000001")), new SizeF(1000, 1414), DateTime.UtcNow, DateTime.UtcNow),
            new Book.Page(PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000002")), new SizeF(1000, 1414), DateTime.UtcNow, DateTime.UtcNow)
        ]) }
    };
}
