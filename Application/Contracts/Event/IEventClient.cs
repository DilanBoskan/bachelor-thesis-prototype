using Domain.Aggregates.Pages;

namespace Application.Contracts.Event;

public record PullData(PageId PageId, ReplicationId LatestReplicationId, ReplicationId RequestedFromReplicationId, byte[] Events);

public interface IEventClient {
    Task<PullData> PullAsync(PageId pageId, ReplicationId fromReplicationId);
    Task<ReplicationId> PushAsync(PageId pageId, ReplicationId latestReplicationId, byte[] data);
}
