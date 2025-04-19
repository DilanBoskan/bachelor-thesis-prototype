using Domain.Entities.Elements;
using Domain.Messages;
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
            // Configure any other options you need here
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // ... other options
            TypeInfoResolver = AppJsonSerializerContext.Default // Use your source generator context
        };
        var jsonContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions);

        services
            .AddScoped<MessageDispatcher>(sp => new MessageDispatcher(Guid.NewGuid(), sp.GetRequiredService<IEventsClient>()))
            .AddScoped<IMessageListener>(sp => sp.GetRequiredService<MessageDispatcher>())
            .AddScoped<IMessagePublisher>(sp => sp.GetRequiredService<MessageDispatcher>())
            .AddScoped<IElementRepository, InMemoryElementRepository>();


        services
            .AddRefitClient<IEventsClient>(new RefitSettings() {
                ContentSerializer = jsonContentSerializer,
            })
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5244"));

        return services;
    }
}