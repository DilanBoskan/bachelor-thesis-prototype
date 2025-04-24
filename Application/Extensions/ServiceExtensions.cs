using Application.Contracts.Command;
using Application.Contracts.Event;
using Application.Contracts.Services;
using Application.Features.Books;
using Application.Features.Elements;
using Application.Features.Elements.Commands;
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
            // Services
            .AddScoped<IBookService, DummyBookService>()
            .AddScoped<IPageService, PageService>()
            .AddScoped<IElementService, ElementService>();

        return services;
    }
}
