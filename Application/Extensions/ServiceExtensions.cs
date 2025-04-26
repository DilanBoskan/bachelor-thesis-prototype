using Application.Contracts.Command;
using Application.Contracts.Event;
using Application.Contracts.Services;
using Application.Features.Books;
using Application.Features.Elements;
using Application.Features.Merge;
using Application.Features.Pages;
using Application.Features.Pages.Commands;
using Application.Features.Replication;
using Domain.Aggregates.Elements.InkStrokes;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services
            // Command Handlers
            .AddScoped<ICommandHandler<CreateInkStrokeElementInPageCommand, InkStrokeElement>, CreateElementInPageCommandHandler>()
            .AddScoped<ICommandHandler<RemoveElementFromPageCommand>, RemoveElementFromPageCommandHandler>()
            // Replication
            .AddScoped<IEventHandler, EventStorage>()
            .AddScoped<IMergeService, MergeService>()
            .AddScoped<IReplicationService, ReplicationService>()
            // Services
            .AddScoped<IBookService, BookService>()
            .AddScoped<IPageService, PageService>()
            .AddScoped<IElementService, ElementService>();

        return services;
    }
}
