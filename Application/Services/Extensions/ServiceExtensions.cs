using Application.Services.Books;
using Application.Services.Events;
using Application.Services.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services
            .AddScoped<IEventManagerFactory, EventManagerFactory>()
            .AddScoped<IEventManager, EventManager>()
            .AddScoped<IBookService, DummyBookService>()
            .AddScoped<IPageService, PageService>();

        return services;
    }
}
