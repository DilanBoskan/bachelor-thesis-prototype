using Domain.Aggregates.Books;
using Presentation.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.ViewModels;

public partial class MainPageViewModel(IBookModelService bookService) : ObservableObjectWithResources {
    private readonly IBookModelService _bookService = bookService;


    public async Task ActivateAsync(BookId bookId, CancellationToken ct = default) {
        _bookId = bookId;
        await ActivateAsync(ct);
    }
    public override Task ActivateAsync(CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(_bookId);

        return base.ActivateAsync(ct);
    }

    protected override async Task CreateResourcesAsync(CancellationToken ct) {
        Book = await _bookService.GetByIdAsync(_bookId, ct);

        await Book.ActivateAsync(ct);
    }

    protected override async Task ReleaseResourcesAsync() {
        if (Book is not null) {
            await Book.DeactivateAsync();
        }
        Book = null;
    }

    private BookId _bookId = null!;
}
