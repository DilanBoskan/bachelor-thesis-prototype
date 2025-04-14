using Domain.Entities.Elements;
using Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;
public static class ServiceExtensions {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) {
        services
            .AddSingleton<IElementRepository, InMemoryElementRepository>();

        return services;
    }
}
