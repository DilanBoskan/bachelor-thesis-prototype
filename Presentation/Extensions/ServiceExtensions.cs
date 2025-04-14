using Application.Services.Books;
using Application.Services.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Services.Books;
using Presentation.Services.Pages;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddWindows(this IServiceCollection services) {
        services
            .AddLogging(builder => {
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            })
            .AddSingleton<IWindowsBookService, WindowsBookService>()
            .AddSingleton<IWindowsPageService, WindowsPageService>()
            .AddSingleton<MainPageViewModel>();

        return services;
    }
}
