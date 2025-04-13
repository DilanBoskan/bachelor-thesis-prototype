using Domain.Entities.Books;
using Domain.Entities.Pages;
using Domain.Helpers;
using System.Drawing;

namespace Application.Services.Books;
public sealed class DummyBookService : IBookService {
    public async Task<Book> GetAsync(BookId id, CancellationToken ct = default) {
        await DelayHelper.Short(ct);

        return new Book(id);
    }

    public async Task<BookContent> GetContentAsync(BookId id, CancellationToken ct = default) {
        await DelayHelper.Short(ct);

        return BOOK_CONTENT;
    }

    private static readonly BookContent BOOK_CONTENT = new([
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
        new Page(PageId.New(), new SizeF(1000, 1414)),
    ]);
}
