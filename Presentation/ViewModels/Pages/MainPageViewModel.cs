using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.Books;
using Presentation.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.ViewModels;

public partial class MainPageViewModel : ObservableObjectWithResources<BookId> {
    [ObservableProperty]
    public partial BookId? BookId { get; private set; }

    protected override Task CreateResourcesAsync(BookId args, CancellationToken ct) {
        BookId = args;

        return Task.CompletedTask;
    }

    protected override Task ReleaseResourcesAsync() {
        BookId = null;

        return Task.CompletedTask;
    }
}
