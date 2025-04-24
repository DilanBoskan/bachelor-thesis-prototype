using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Extensions;
using Presentation.Services.Pages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Page;

public sealed partial class PageModel(Domain.Aggregates.Pages.Page page) : ObservableObjectWithResources {
    private readonly IPageModelService _pageService = App.Current.ServiceProvider.GetRequiredService<IPageModelService>();
    private readonly ILogger<PageModel> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<PageModel>>();

    public PageId Id { get; } = page.Id;
    public BookId BookId { get; } = page.BookId;
    public SizeF Size { get; } = page.Size;
    public DateTime CreatedAt { get; } = page.CreatedAt;
    public DateTime UpdatedAt { get; } = page.UpdatedAt;

    #region UI-Specific
    public InkStrokeContainer StrokeContainer { get; } = new InkStrokeContainer();
    #endregion

    #region State Management
    protected override async Task CreateResourcesAsync(CancellationToken ct = default) {
        _logger.LogDebug("Loading page {PageId}", Id.Value);

        // Load current state of the page
        var content = await _pageService.GetContentAsync(Id, ct);

        RegisterMessageHandlers();

        // UI
        AddElements(content.Elements);
    }
    protected override Task ReleaseResourcesAsync() {
        _logger.LogDebug("Unloading page {PageId}", Id.Value);

        UnregisterMessagesHandlers();

        // UI
        RemoveAllElements();

        return Task.CompletedTask;
    }
    #endregion


    [RelayCommand]
    private async Task StrokesCollectedAsync(IReadOnlyList<InkStroke> inkStrokes) {

        foreach (var inkStroke in inkStrokes) {
            var createdAt =  inkStroke.StrokeStartedTime.GetValueOrDefault(DateTimeOffset.UtcNow).DateTime.ToUniversalTime();
            var points = inkStroke.ToInkStrokePoints();

            await _pageService.CreateInkStrokeElementAsync(BookId, Id, createdAt, points);
        }

        _logger.LogDebug("Strokes collected on page {PageId}", Id.Value);
    }
}
