using Application.Contracts.Event;
using Application.Protos.Pages;
using Domain.Aggregates.Books;
using Domain.Aggregates.Books.Pages;
using Domain.Aggregates.Pages;
using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Repositories.InMemory;
public class InMemoryBookRepository : IBookRepository {
    public Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default) => Task.FromResult(_books.TryGetValue(id, out var page) ? page : null);


    private static readonly Dictionary<BookId, Book> _books = new() {
       { BookId.Create(Guid.Empty), new Book(BookId.Create(Guid.Empty), DateTime.UtcNow, DateTime.UtcNow, [
            PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000001")),
            PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000002"))
        ]) }
    };
}
