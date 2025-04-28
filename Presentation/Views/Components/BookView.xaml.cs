using CommunityToolkit.WinUI;
using Domain.Aggregates.Books;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Presentation.Models.Pages;
using Presentation.ViewModels.Components;
using System;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Presentation.Views.Components;

public sealed partial class BookView : UserControl {
    public BookViewModel ViewModel { get; } = App.Current.ServiceProvider.GetRequiredService<BookViewModel>();

    public BookId? BookId {
        get => (BookId?)GetValue(BookIdProperty);
        set => SetValue(BookIdProperty, value);
    }

    private readonly ILogger<BookView> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<BookView>>();
    public BookView() {
        this.InitializeComponent();

        Loaded += BookView_Loaded;
        Unloaded += BookView_Unloaded;
    }

    #region Component Lifecycle
    private async void BookView_Loaded(object sender, RoutedEventArgs e) {
        if (BookId is not null) {
            await ViewModel.ActivateAsync(BookId);
        }
    }
    private async void BookView_Unloaded(object sender, RoutedEventArgs e) {
        await ViewModel.DeactivateAsync();
    }
    #endregion


    private async void OnBookIdChanged(BookId? oldValue, BookId? newValue) {
        if (newValue is not null) {
            await ViewModel.ActivateAsync(newValue);
        }
    }

    private static void BookIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BookView)?.OnBookIdChanged(e.OldValue as BookId, e.NewValue as BookId);

    public static readonly DependencyProperty BookIdProperty =
        DependencyProperty.Register("BookId", typeof(BookId), typeof(BookView), new PropertyMetadata(null, BookIdChanged));
}
