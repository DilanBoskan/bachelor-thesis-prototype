using Application.Services.Books;
using Domain.Entities.Books;
using Presentation.Extensions;
using Presentation.Models;
using Presentation.Models.Books;
using Presentation.Models.Page;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Books;
public sealed class WindowsBookService(IBookService bookService) : IWindowsBookService {
    private readonly IBookService _bookService = bookService;

    public async Task<BookModel> GetAsync(BookId id, CancellationToken ct = default) {
        var book = await _bookService.GetAsync(id, ct);

        return book.ToBookModel();
    }
    public async Task<WindowsBookContent> GetContentAsync(BookId id, CancellationToken ct = default) {
        var book = await _bookService.GetContentAsync(id, ct);
        var pages = book.Pages
            .Select(p => p.ToPageModel())
            .ToList();

        return new WindowsBookContent(pages);
    }
}
