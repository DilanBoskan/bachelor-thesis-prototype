using Domain.Entities.Elements;
using Domain.Events;
using Infrastructure.Events.Local;
using Infrastructure.Messages.Local;
using Infrastructure.Repositories.InMemory;
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
            .AddScoped<EventDispatcher>(sp => new EventDispatcher(Guid.NewGuid(), sp.GetRequiredService<IEventsClient>()))
            .AddScoped<IEventListener>(sp => sp.GetRequiredService<EventDispatcher>())
            .AddScoped<IEventPublisher>(sp => sp.GetRequiredService<EventDispatcher>())
            .AddScoped<IElementRepository, InMemoryElementRepository>();


        services
            .AddRefitClient<IEventsClient>(new RefitSettings() {
                ContentSerializer = jsonContentSerializer,
            })
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5244"));

        return services;
    }
}