using Domain.Entities.Elements;
using Domain.Messages;
using Infrastructure.Messages.InMemory;
using Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Guid userId) {
        services
            .AddScoped<InMemoryMessageDispatcher>(sp => new InMemoryMessageDispatcher(userId))
            .AddScoped<IMessageListener>(sp => sp.GetRequiredService<InMemoryMessageDispatcher>())
            .AddScoped<IMessagePublisher>(sp => sp.GetRequiredService<InMemoryMessageDispatcher>())
            .AddSingleton<IElementRepository, InMemoryElementRepository>();

        return services;
    }
}
