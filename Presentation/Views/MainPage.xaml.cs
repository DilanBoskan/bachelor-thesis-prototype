using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Presentation.Models.Page;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Presentation.Views;


public sealed partial class MainPage : Page
{
    public MainPageViewModel ViewModel = Ioc.Default.GetRequiredService<MainPageViewModel>();

    private readonly ILogger<MainPage> _logger = Ioc.Default.GetRequiredService<ILogger<MainPage>>();
    public MainPage()
    {
        InitializeComponent();

        Loaded += MainPage_Loaded;
    }

    private async void MainPage_Loaded(object sender, RoutedEventArgs e) {
        await ViewModel.ActivateAsync();
    }

    [RelayCommand]
    private void PrintLoadedPages() {
        ViewModel.Book?.Pages.ToList().ForEach(page => {
            if (page.IsActive) {
                _logger.LogInformation("Page is active {pageId}", page.Id.Value);
            }
        });
    }

    private async void ItemsRepeater_ElementPrepared(ItemsRepeater _, ItemsRepeaterElementPreparedEventArgs args) {
        var pageModel = (PageModel)((FrameworkElement)args.Element).DataContext;

        ActivateUIForPage(args.Element, pageModel);

        _logger.LogDebug("Activating page {PageId}", pageModel.Id.Value);

        await pageModel.ActivateAsync();
    }
    private void ItemsRepeater_ElementIndexChanged(ItemsRepeater _, ItemsRepeaterElementIndexChangedEventArgs args) {
        var pageModel = (PageModel)((FrameworkElement)args.Element).DataContext;

        DeactivateUIForPage(args.Element);
        ActivateUIForPage(args.Element, pageModel);

        _logger.LogDebug("Transferring page model {PageId}", pageModel.Id.Value);
    }

    private async void ItemsRepeater_ElementClearing(ItemsRepeater _, ItemsRepeaterElementClearingEventArgs args) {
        var pageModel = (PageModel)((FrameworkElement)args.Element).DataContext;

        DeactivateUIForPage(args.Element);

        _logger.LogDebug("Deactivating page {PageId}", pageModel.Id.Value);

        await pageModel.DeactivateAsync();
    }

    private void ActivateUIForPage(UIElement element, PageModel pageModel) {
        var inkCanvas = element.FindDescendant<InkCanvas>()!;

        ArgumentNullException.ThrowIfNull(pageModel);
        ArgumentNullException.ThrowIfNull(inkCanvas);
        if (_inkCanvasEventHandlers.ContainsKey(inkCanvas)) throw new ArgumentException("InkCanvas already has handlers registered");

        inkCanvas.InkPresenter.InputDeviceTypes =
            Windows.UI.Core.CoreInputDeviceTypes.Mouse |
            Windows.UI.Core.CoreInputDeviceTypes.Pen |
            Windows.UI.Core.CoreInputDeviceTypes.Touch;
        inkCanvas.InkPresenter.StrokeContainer = pageModel.StrokeContainer;

        var strokesCollectedHandler = new TypedEventHandler<InkPresenter, InkStrokesCollectedEventArgs>(
            async (s, a) => await pageModel.StrokesCollectedCommand.ExecuteAsync(a.Strokes)
        );
        var events = new InkCanvasEventHandlers(strokesCollectedHandler);

        inkCanvas.InkPresenter.StrokesCollected += events.StrokesCollected;
        _inkCanvasEventHandlers.Add(inkCanvas, events);
    }
    private void DeactivateUIForPage(UIElement element) {
        var inkCanvas = element.FindDescendant<InkCanvas>()!;

        ArgumentNullException.ThrowIfNull(inkCanvas);
        if (!_inkCanvasEventHandlers.TryGetValue(inkCanvas, out InkCanvasEventHandlers? events)) throw new ArgumentException("InkCanvas not found in event handlers");

        inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.None;
        inkCanvas.InkPresenter.StrokeContainer = null;
        inkCanvas.InkPresenter.StrokesCollected -= events.StrokesCollected;
        _inkCanvasEventHandlers.Remove(inkCanvas);
    }

    private readonly Dictionary<InkCanvas, InkCanvasEventHandlers> _inkCanvasEventHandlers = [];
    private record InkCanvasEventHandlers(TypedEventHandler<InkPresenter, InkStrokesCollectedEventArgs> StrokesCollected);
}
