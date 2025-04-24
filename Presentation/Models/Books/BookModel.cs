using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.Books;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Models.Page;
using Presentation.Services.Books;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Models.Books;

public partial class BookModel(Book book) : ObservableObjectWithResources {
    private readonly IBookModelService _bookService = App.Current.ServiceProvider.GetRequiredService<IBookModelService>();
    private readonly ILogger<BookModel> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<BookModel>>();

    public BookId Id { get; } = book.Id;
    public DateTime CreatedAt { get; } = book.CreatedAt;
    public DateTime UpdatedAt { get; } = book.UpdatedAt;

    [ObservableProperty]
    public partial ObservableCollection<PageModel> Pages { get; set; } = [];

    protected override async Task CreateResourcesAsync(CancellationToken ct) {
        var content = await _bookService.GetContentAsync(Id, ct);

        // UI
        Pages = [.. content.Pages];
    }

    protected override Task ReleaseResourcesAsync() {
        // UI
        Pages = [];

        return Task.CompletedTask;
    }

}
