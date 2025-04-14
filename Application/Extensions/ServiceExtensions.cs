using Application.Services.Books;
using Application.Services.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services
            .AddSingleton<IBookService, DummyBookService>()
            .AddSingleton<IPageService, PageService>();

        return services;
    }
}
