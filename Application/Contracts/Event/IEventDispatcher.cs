using Domain.Events;

namespace Application.Contracts.Event;
public interface IEventDispatcher {
    Task PublishAsync(IReadOnlyList<Domain.Events.Event> events, CancellationToken ct = default);
}
