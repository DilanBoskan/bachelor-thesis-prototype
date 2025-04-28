using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Domain.Aggregates.Books;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Uri = System.Uri;

namespace Presentation.Views;


public sealed partial class MainPage : Page
{
    public MainPageViewModel ViewModel = App.Current.ServiceProvider.GetRequiredService<MainPageViewModel>();

    private readonly ILogger<MainPage> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<MainPage>>();
    public MainPage()
    {
        InitializeComponent();
    }

    #region Page Lifecycle
    protected async override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);

        if (e.Parameter is not BookId bookId) {
            bookId = BookId.New();
        }

        await ViewModel.ActivateAsync(bookId);
    }
    protected async override void OnNavigatedFrom(NavigationEventArgs e) {
        base.OnNavigatedFrom(e);

        await ViewModel.DeactivateAsync();
    }
    #endregion

    [RelayCommand]
    private async Task NewWindowAsync() {
        ArgumentNullException.ThrowIfNull(ViewModel.BookId);

        var uri = new Uri($"ba-collaboration://{ViewModel.BookId.Value}");
        await Launcher.LaunchUriAsync(uri);
    }


    private record InkCanvasEventHandlers(TypedEventHandler<InkPresenter, InkStrokesCollectedEventArgs> StrokesCollected);
}
