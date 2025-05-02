using Application.Contracts.Event;
using Application.Contracts.Services;
using Application.Features.Books;
using Application.Features.Elements;
using Application.Features.Merge;
using Application.Features.Pages;
using Application.Features.Replication;
using Domain.Aggregates.Elements.InkStrokes;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services
            // Replication
            .AddSingleton<EventStorage>()
            .AddTransient<IEventHandler, EventStorage>(prov => prov.GetRequiredService<EventStorage>())
            .AddTransient<IMergeService, MergeService>()
            .AddTransient<IReplicationService, ReplicationService>()
            // Services
            .AddTransient<IBookService, BookService>()
            .AddTransient<IPageService, PageService>()
            .AddTransient<IElementService, ElementService>();

        return services;
    }
}
