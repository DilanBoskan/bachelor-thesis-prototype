using Domain.Events;

namespace Application.Contracts.Event;

public interface ICloudEventDispatcher {
    Task PublishAsync(IReadOnlyList<IEvent> events, CancellationToken ct = default);
}
