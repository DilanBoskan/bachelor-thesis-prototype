using Application.Extensions;
using Application.Services.Books;
using Application.Services.Pages;
using CommunityToolkit.Mvvm.DependencyInjection;
using Domain.Entities.Books;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Extensions;
using Presentation.Services.Books;
using Presentation.Services.Collaboration;
using Presentation.Services.Pages;
using Presentation.ViewModels;
using Presentation.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Presentation;

/// <summary>
/// Provides application-specific behavior to supplement the default <see cref="Application"/> class.
/// </summary>
public sealed partial class App : Windows.UI.Xaml.Application {
    public IServiceProvider ServiceProvider { get; } = ConfigureServiceProvider();

    public new static App Current => (App)Windows.UI.Xaml.Application.Current;

    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        Suspending += OnSuspending;
    }

    private static ServiceProvider ConfigureServiceProvider() {
        var serviceProvider = new ServiceCollection()
            .AddInfrastructure()
            .AddApplication()
            .AddWindows() // UI Layer
            .BuildServiceProvider();

        return serviceProvider;
    }

    /// <inheritdoc/>
    protected override void OnLaunched(LaunchActivatedEventArgs e) => OnActivated(e);
    protected override async void OnActivated(IActivatedEventArgs args) {
        BookId bookId = BookId.New();
        if (args is ProtocolActivatedEventArgs protocolArgs) {
            if (Guid.TryParse(protocolArgs.Uri.Host, out var id)) {
                bookId = BookId.Create(id);
            }
        }

        if (Window.Current.Content is not Frame) {
            Frame rootFrame = new();
            Window.Current.Content = rootFrame;
            if (rootFrame.Content is null) {
                rootFrame.Navigate(typeof(MainPage), bookId);
            }
            Window.Current.Activate();
        }

        Activate(bookId);
    }
    public void Activate(BookId bookId) {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        // Register background thread
        _ = new CollaborationHelper().StartAsync(bookId, _cts.Token);
    }

    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

        _cts.Cancel();

        deferral.Complete();
    }

    private CancellationTokenSource _cts = new();
}
