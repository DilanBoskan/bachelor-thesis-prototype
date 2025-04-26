using Domain.Aggregates.Books;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ViewModels.Components;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Presentation.Views.Components;

public sealed partial class BookView : UserControl {
    public BookViewModel ViewModel { get; } = App.Current.ServiceProvider.GetRequiredService<BookViewModel>();

    public BookId? BookId {
        get => (BookId?)GetValue(BookIdProperty);
        set => SetValue(BookIdProperty, value);
    }

    public BookView() {
        this.InitializeComponent();
    }

    private async void OnBookIdChanged(BookId? oldValue, BookId? newValue) {
        if (newValue is not null) {
            await ViewModel.ActivateAsync(newValue);
        }
    }

    private static void BookIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BookView)?.OnBookIdChanged(e.OldValue as BookId, e.NewValue as BookId);

    public static readonly DependencyProperty BookIdProperty =
        DependencyProperty.Register("BookId", typeof(BookId), typeof(BookView), new PropertyMetadata(null, BookIdChanged));
}
