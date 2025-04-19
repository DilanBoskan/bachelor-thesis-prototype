using Application.Services.Books;
using Application.Services.Messages;
using Application.Services.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services
            .AddScoped<IMessageManagerFactory, MessageManagerFactory>()
            .AddScoped<IMessageManager, MessageManager>()
            .AddScoped<IBookService, DummyBookService>()
            .AddScoped<IPageService, PageService>();

        return services;
    }
}
