using Application.Services.Books;
using Domain.Entities.Books;
using Presentation.Models;
using Presentation.Models.Page;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Books;
public sealed class WindowsBookService(IBookService bookService) : IWindowsBookService {
    private readonly IBookService _bookService = bookService;

    public async Task<BookModel> GetAsync(BookId id, CancellationToken ct = default) {
        var book = await _bookService.GetAsync(id, ct);

        return MapBook(book);
    }
    public async Task<WindowsBookContent> GetContentAsync(BookId id, CancellationToken ct = default) {
        var book = await _bookService.GetContentAsync(id, ct);
        var pages = book.Pages
            .Select(MapPage)
            .ToList();

        return new WindowsBookContent(pages);
    }

    private static BookModel MapBook(Book book) => new(book);
    private static PageModel MapPage(Domain.Entities.Pages.Page page) => new(page);
}
