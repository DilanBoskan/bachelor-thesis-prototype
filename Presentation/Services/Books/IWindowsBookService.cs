using Domain.Entities.Books;
using Presentation.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Books;

public interface IWindowsBookService {
    Task<BookModel> GetAsync(BookId id, CancellationToken ct = default);
    Task<WindowsBookContent> GetContentAsync(BookId id, CancellationToken ct = default);
}
