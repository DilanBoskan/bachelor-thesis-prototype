using Domain.Events;

namespace Application.Contracts.Event;
public interface IEventDispatcher {
    Task PublishAsync(IReadOnlyList<IEvent> events, CancellationToken ct = default);
}
