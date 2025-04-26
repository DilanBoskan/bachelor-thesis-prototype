using Application.Contracts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.Books;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Models.Books;

public partial class BookModel(BookId bookId) : ObservableObjectWithResources {
    private readonly IBookService _bookService = App.Current.ServiceProvider.GetRequiredService<IBookService>();

    public BookId Id { get; } = bookId;
    [ObservableProperty]
    public partial DateTime CreatedAt { get; private set; }
    [ObservableProperty]
    public partial DateTime UpdatedAt { get; private set; }
    [ObservableProperty]
    public partial ObservableCollection<Book.Page> Pages { get; private set; }


    protected override async Task CreateResourcesAsync(CancellationToken ct) {
        var book = await _bookService.GetByIdAsync(Id, ct);
        ArgumentNullException.ThrowIfNull(book);

        CreatedAt = book.CreatedAt;
        UpdatedAt = book.UpdatedAt;
        Pages = new ObservableCollection<Book.Page>(book.Pages);
    }
    protected override Task ReleaseResourcesAsync() {
        CreatedAt = default;
        UpdatedAt = default;
        Pages = [];

        return Task.CompletedTask;
    }
}
