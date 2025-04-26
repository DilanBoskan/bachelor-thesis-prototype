using CommunityToolkit.Mvvm.Input;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Pages;

public sealed partial class PageModel(PageId pageId) : ObservableObjectWithResources {
    private readonly IPageModelService _pageService = App.Current.ServiceProvider.GetRequiredService<IPageModelService>();
    private readonly ILogger<PageModel> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<PageModel>>();

    public PageId Id { get; } = pageId.Id;
    public SizeF Size { get; } = pageId.Size;
    public DateTime CreatedAt { get; } = pageId.CreatedAt;
    public DateTime UpdatedAt { get; } = pageId.UpdatedAt;

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
