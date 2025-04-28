using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Services.Cloud;
using Presentation.ViewModels;

namespace Presentation.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddWindows(this IServiceCollection services) {
        services
            .AddLogging(builder => {
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .AddScoped<CloudSyncingService>()
            .AddScoped<MainPageViewModel>();

        return services;
    }
}
