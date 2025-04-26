using Domain.Aggregates.Pages;

namespace Domain.Events;

public interface IEvent {
    DateTime OccurredAt { get; }
    PageId PageId { get; }
    ReplicationId ReplicationId { get; }
}
