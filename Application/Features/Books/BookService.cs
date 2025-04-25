using Application.Contracts.Services;
using Application.Helpers;
using Domain.Aggregates.Books;
using Domain.Aggregates.Books.Pages;
using Domain.Aggregates.Pages;
using System.Drawing;

namespace Application.Features.Books;
public sealed class BookService(IBookRepository bookRepository) : IBookService {
    public Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default) => bookRepository.GetByIdAsync(id, ct);

}
