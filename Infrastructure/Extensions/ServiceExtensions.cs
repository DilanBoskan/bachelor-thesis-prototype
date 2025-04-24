using Application.Contracts.Cloud;
using Application.Contracts.Command;
using Application.Contracts.Event;
using Domain.Aggregates;
using Domain.Aggregates.Elements;
using Domain.Events;
using Infrastructure.Cloud;
using Infrastructure.Command;
using Infrastructure.Event;
using Infrastructure.Persistance.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Infrastructure.Extensions;

public static class ServiceExtensions {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, UserId userId, Guid instanceId) {
        var jsonSerializerOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = AppJsonSerializerContext.Default // Use your source generator context
        };
        var jsonContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions);

        services
            // Command/Events
            .AddScoped<ICommandDispatcher, CommandDispatcher>()
            .AddScoped<IEventDispatcher, EventDispatcher>()
            .AddScoped<ICloudEventDispatcher, EventDispatcher>()
            .AddSingleton<EventAggregator>()
            .AddScoped<IEventHandler, EventAggregator>(prov => prov.GetRequiredService<EventAggregator>())
            .AddScoped<ICloudManagerFactory, CloudManagerFactory>()
            // Repositories
            .AddScoped<IElementRepository, InMemoryElementRepository>(prov => new InMemoryElementRepository(userId, prov.GetRequiredService<IEventDispatcher>()));


        services
            .AddRefitClient<IEventsClient>(new RefitSettings() {
                ContentSerializer = jsonContentSerializer,
            })
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5244"));

        return services;
    }
}