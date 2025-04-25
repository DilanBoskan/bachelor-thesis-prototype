using Application.Contracts.Services;
using Domain.Aggregates.Books;
using Presentation.Extensions;
using Presentation.Models;
using Presentation.Models.Books;
using Presentation.Models.Page;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Books;
public sealed class BookModelService(IBookService bookService) : IBookModelService {
    private readonly IBookService _bookService = bookService;

    public async Task<BookModel> GetAsync(BookId id, CancellationToken ct = default) {
        var book = await _bookService.GetByIdAsync(id, ct);

        return book.ToWindows();
    }
    public async Task<BookModelContent> GetContentAsync(BookId id, CancellationToken ct = default) {
        var book = await _bookService.GetContentAsync(id, ct);
        var pages = book.Pages
            .Select(p => p.ToWindows())
            .ToList();

        return new BookModelContent(pages);
    }
}
