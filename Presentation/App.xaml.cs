using Application.Extensions;
using Application.Helpers.Concurrency;
using Domain.Aggregates.Books;
using Domain.Aggregates.Users;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Extensions;
using Presentation.Services.Cloud;
using Presentation.Views;
using System;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Presentation;

/// <summary>
/// Provides application-specific behavior to supplement the default <see cref="Application"/> class.
/// </summary>
public sealed partial class App : Windows.UI.Xaml.Application {
    public UserId UserId { get; } = UserId.Create(Guid.Parse("00000000-0000-0000-0000-000000000001"));
    public Guid InstanceId { get; } = Guid.NewGuid();
    public DispatcherQueue UIScheduler { get; } = DispatcherQueue.GetForCurrentThread();
    public IScheduler DatabaseScheduler { get; } = new DatabaseThreadScheduler(new SingleThreadTaskScheduler());
    public IServiceProvider ServiceProvider { get; }

    public new static App Current => (App)Windows.UI.Xaml.Application.Current;

    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        ServiceProvider = ConfigureServiceProvider();

        Suspending += OnSuspending;
    }

    private ServiceProvider ConfigureServiceProvider() {
        var serviceProvider = new ServiceCollection()
            .AddInfrastructure(UserId, InstanceId)
            .AddApplication()
            .AddWindows() // UI Layer
            .BuildServiceProvider();

        return serviceProvider;
    }

    /// <inheritdoc/>
    protected override void OnLaunched(LaunchActivatedEventArgs e) => OnActivated(e);

    protected override void OnActivated(IActivatedEventArgs args) {
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
        var syncingService = App.Current.ServiceProvider.GetRequiredService<CloudSyncingService>();
        _ = syncingService.StartAsync(InstanceId, bookId, _cts.Token);
    }

    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

        _cts.Cancel();

        deferral.Complete();
    }

    private CancellationTokenSource _cts = new();
}
