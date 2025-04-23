using Application.Contracts.Command;
using Application.Contracts.Event;
using Domain.Aggregates.Elements;
using Domain.Events;
using Infrastructure.Command;
using Infrastructure.Event;
using Infrastructure.Persistance.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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
            // Repositories
            .AddScoped<IElementRepository, InMemoryElementRepository>();


        services
            .AddRefitClient<IEventsClient>(new RefitSettings() {
                ContentSerializer = jsonContentSerializer,
            })
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5244"));

        return services;
    }
}