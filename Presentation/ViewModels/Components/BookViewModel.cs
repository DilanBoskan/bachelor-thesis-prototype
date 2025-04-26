using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.Books;
using Presentation.Models;
using Presentation.Models.Books;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.ViewModels.Components;


public partial class BookViewModel : ObservableObjectWithResources<BookId> {
    [ObservableProperty]
    public partial BookModel? Book { get; private set; }

    protected override async Task CreateResourcesAsync(BookId bookId, CancellationToken ct) {
        Book = new BookModel(bookId);
        await Book.ActivateAsync(ct);
    }

    protected override async Task ReleaseResourcesAsync() {
        if (Book is not null) {
            await Book.DeactivateAsync();
            Book = null;
        }
    }
}
