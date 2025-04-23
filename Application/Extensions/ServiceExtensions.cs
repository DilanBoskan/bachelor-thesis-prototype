using Application.Contracts.Command;
using Application.Contracts.Event;
using Application.Contracts.Services;
using Application.Features.Books;
using Application.Features.Elements.Commands;
using Application.Features.Elements.Events;
using Application.Features.Pages;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services
            // Command Handlers
            .AddScoped<ICommandHandler<CreateInkStrokeElementCommand, InkStrokeElement>, CreateElementCommandHandler>()
            .AddScoped<ICommandHandler<DeleteElementCommand>, DeleteElementCommandHandler>()
            // Event Handlers
            .AddScoped<IEventHandler<ElementDeletedEvent>, ElementDeletedEventHandler>()
            .AddScoped<IEventHandler<ElementCreatedEvent>, ElementCreatedEventHandler>()
            // Services
            .AddScoped<IBookService, DummyBookService>()
            .AddScoped<IPageService, PageService>();

        return services;
    }
}
