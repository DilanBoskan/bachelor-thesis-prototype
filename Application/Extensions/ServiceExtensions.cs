using Application.Services.Books;
using Application.Services.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services) {
        services
            .AddSingleton<IBookService, DummyBookService>()
            .AddSingleton<IPageService, DummyPageService>();

        return services;
    }
}
