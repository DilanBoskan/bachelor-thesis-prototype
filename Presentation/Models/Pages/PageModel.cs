using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Domain.Entities.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Services.Pages;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Page;

public sealed partial class PageModel(Domain.Entities.Pages.Page page) : ObservableObjectWithResources {
    private readonly IWindowsPageService _pageService = App.Current.ServiceProvider.GetRequiredService<IWindowsPageService>();
    private readonly ILogger<PageModel> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<PageModel>>();

    public PageId Id { get; } = page.Id;
    public SizeF Size { get; } = page.Size;

    #region UI-Specific
    public InkStrokeContainer StrokeContainer { get; } = new InkStrokeContainer();
    #endregion

    #region State Management
    protected override async Task CreateResourcesAsync(CancellationToken ct = default) {
        _logger.LogDebug("Loading page {PageId}", Id.Value);

        // Load current state of the page
        var content = await _pageService.GetContentAsync(Id, ct);

        // UI
        StrokeContainer.AddStrokes(content.InkStrokes);
    }
    protected override Task ReleaseResourcesAsync() {
        _logger.LogDebug("Unloading page {PageId}", Id.Value);

        // UI
        StrokeContainer.Clear();

        return Task.CompletedTask;
    }
    #endregion


    [RelayCommand]
    private void StrokesCollected(IReadOnlyList<InkStroke> inkStrokes) {
        _logger.LogDebug("Strokes collected on page {PageId}", Id.Value);
    }
}
