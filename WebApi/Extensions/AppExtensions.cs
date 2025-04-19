using System.Reflection;
using WebApi.EndpointDefinitions;

namespace WebApi.Extensions;

public static class AppExtensions {
    public static WebApplication UseEndpointDefintions(this WebApplication app) {
        // Get all types implementing IEndpointDefinition from the current assembly
        var endpointDefinitions = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        // Call MapDefinitions on each instance
        foreach (var endpointDefinition in endpointDefinitions) {
            endpointDefinition.MapDefinitions(app);
        }

        return app;
    }
}
