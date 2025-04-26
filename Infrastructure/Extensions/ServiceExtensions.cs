using Application.Contracts.Command;
using Application.Contracts.Event;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using Infrastructure.Command;
using Infrastructure.Event;
using Infrastructure.Persistance.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;

namespace Infrastructure.Extensions;

public static class ServiceExtensions {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) {
        var jsonSerializerOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = AppJsonSerializerContext.Default // Use your source generator context
        };
        var jsonContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions);

        services
            // Command/Events
            .AddScoped<ICommandDispatcher, CommandDispatcher>()
            .AddScoped<IEventDispatcher, EventDispatcher>()
            .AddScoped<IEventClient, EventClient>()
            // Repositories
            .AddScoped<IBookRepository, InMemoryBookRepository>()
            .AddScoped<IPageRepository, InMemoryPageRepository>();


        services
            .AddRefitClient<IHttpEventClient>(new RefitSettings() {
                ContentSerializer = jsonContentSerializer,
            })
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5244"));

        return services;
    }
}