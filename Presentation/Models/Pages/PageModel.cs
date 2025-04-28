using Application.Contracts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Pages;

public sealed partial class PageModel(PageId id) : ObservableObjectWithResources {
    private readonly IPageService _pageService = App.Current.ServiceProvider.GetRequiredService<IPageService>();
    private readonly ILogger<PageModel> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<PageModel>>();

    public PageId Id { get; } = id;

    [ObservableProperty]
    public partial SizeF Size { get; private set; }
    [ObservableProperty]
    public partial DateTime CreatedAt { get; private set; }
    [ObservableProperty]
    public partial DateTime UpdatedAt { get; private set; }

    #region UI-Specific
    public InkStrokeContainer StrokeContainer { get; } = new InkStrokeContainer();
    #endregion

    #region State Management
    protected override async Task CreateResourcesAsync(CancellationToken ct = default) {
        _logger.LogDebug("Loading page {PageId}", Id.Value);

        var page = await _pageService.GetByIdAsync(Id, ct);
        ArgumentNullException.ThrowIfNull(page);

        // Load current state of the page
        var elements = page.Elements.Select(e => e.ToWindows());

        RegisterMessageHandlers();

        // UI
        Size = page.Size;
        CreatedAt = page.CreatedAt;
        UpdatedAt = page.UpdatedAt;
        AddElements(elements);
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

            await _pageService.CreateInkStrokeElementAsync(Id, createdAt, points);
        }

        _logger.LogDebug("Strokes collected on page {PageId}", Id.Value);
    }
}
