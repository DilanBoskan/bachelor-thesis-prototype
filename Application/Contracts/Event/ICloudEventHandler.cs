using Domain.Events;

namespace Application.Contracts.Event;

/// <summary>
/// Handler for events from the cloud
/// </summary>
public interface ICloudEventHandler {
    Task HandleAsync(IReadOnlyList<IEvent> events);
}
