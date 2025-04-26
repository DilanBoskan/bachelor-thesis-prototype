using Domain.Events;

namespace Application.Contracts.Event;

/// <summary>
/// Handler for internal events
/// </summary>
public interface IEventHandler {
    Task HandleAsync(IReadOnlyList<IEvent> events);
}
