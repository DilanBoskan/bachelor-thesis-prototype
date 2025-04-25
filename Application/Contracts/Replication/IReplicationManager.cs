using Domain.Events;

namespace Application.Contracts.Replication;

public interface IReplicationManager {
    Task<IReadOnlyList<IEvent>> PullAsync();
    Task PushAsync();
}