using Application.Contracts.Services;
using Application.Helpers;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using System.Drawing;

namespace Application.Features.Books;
public sealed class DummyBookService : IBookService {
    public async Task<Book> GetAsync(BookId id, CancellationToken ct = default) {
        await DelayHelper.Short(ct);

        return Book.Load(id, DateTime.UtcNow, DateTime.UtcNow);
    }

    public async Task<BookContent> GetContentAsync(BookId id, CancellationToken ct = default) {
        await DelayHelper.Short(ct);

        return BOOK_CONTENT;
    }

    private static readonly BookContent BOOK_CONTENT = new([
        Page.Load(PageId.Create(Guid.Empty), BookId.Create(Guid.Empty), new SizeF(1000, 1414), DateTime.UtcNow, DateTime.UtcNow),
    ]);
}
