using Domain.Aggregates.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.ViewModels.Components;
using System;
using System.ComponentModel;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Presentation.Views.Components;
public sealed partial class PageView : UserControl {
    public PageViewModel ViewModel { get; } = App.Current.ServiceProvider.GetRequiredService<PageViewModel>();

    public PageId? PageId {
        get => (PageId?)GetValue(PageIdProperty);
        set => SetValue(PageIdProperty, value);
    }

    private readonly ILogger<PageView> _logger = App.Current.ServiceProvider.GetRequiredService<ILogger<PageView>>();
    public PageView() {
        this.InitializeComponent();

        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        InkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
        InkCanvas.InkPresenter.InputDeviceTypes =
            Windows.UI.Core.CoreInputDeviceTypes.Mouse |
            Windows.UI.Core.CoreInputDeviceTypes.Pen |
            Windows.UI.Core.CoreInputDeviceTypes.Touch;

        Loaded += PageView_Loaded; ;
        Unloaded += PageView_Unloaded;
    }

    #region Component Lifecycle
    private async void PageView_Loaded(object sender, RoutedEventArgs e) {
        if (PageId is not null) {
            await ViewModel.ActivateAsync(PageId);
        }
    }
    private async void PageView_Unloaded(object sender, RoutedEventArgs e) {
        await ViewModel.DeactivateAsync();
    }
    #endregion

    #region Events
    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        switch (e.PropertyName) {
            case nameof(PageViewModel.Page):
                InkCanvas.InkPresenter.StrokeContainer = ViewModel.Page?.StrokeContainer;
                break;
        }
    }

    private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args) => ViewModel.Page?.StrokesCollectedCommand.Execute(args.Strokes);
    #endregion

    #region Property Changes
    private async void OnPageIdChanged(PageId? oldValue, PageId? newValue) {
        if (newValue is not null) {
            await ViewModel.ActivateAsync(newValue);
        }
    }
    #endregion

    private static void PageIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as PageView)?.OnPageIdChanged(e.OldValue as PageId, e.NewValue as PageId);

    public static readonly DependencyProperty PageIdProperty =
        DependencyProperty.Register("PageId", typeof(PageId), typeof(PageView), new PropertyMetadata(null, PageIdChanged));
}
