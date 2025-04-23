using Domain.Aggregates.Books;
using Presentation.Models;
using Presentation.Models.Books;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Books;

public interface IBookModelService {
    Task<BookModel> GetAsync(BookId id, CancellationToken ct = default);
    Task<BookModelContent> GetContentAsync(BookId id, CancellationToken ct = default);
}
