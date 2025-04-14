using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Entities.Books;
using Presentation.Models;
using Presentation.Models.Books;
using Presentation.Services.Books;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.ViewModels;

public partial class MainPageViewModel(IWindowsBookService bookService) : ObservableObjectWithResources {
    private readonly IWindowsBookService _bookService = bookService;

    [ObservableProperty]
    public partial BookModel? Book { get; set; }

    protected override async Task CreateResourcesAsync(CancellationToken ct) {
        Book = await _bookService.GetAsync(BookId.New(), ct);

        await Book.ActivateAsync(ct);
    }

    protected override async Task ReleaseResourcesAsync() {
        if (Book is not null) {
            await Book.DeactivateAsync();
        }
        Book = null;
    }
}
