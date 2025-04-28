using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Services.Cloud;
using Presentation.ViewModels;
using Presentation.ViewModels.Components;

namespace Presentation.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddWindows(this IServiceCollection services) {
        services
            .AddLogging(builder => {
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .AddTransient<PageViewModel>()
            .AddTransient<BookViewModel>()
            .AddTransient<CloudSyncingService>()
            .AddTransient<MainPageViewModel>();

        return services;
    }
}
