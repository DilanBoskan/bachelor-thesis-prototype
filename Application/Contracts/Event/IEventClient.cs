using Domain.Aggregates.Pages;

namespace Application.Contracts.Event;

public class PullData(PageId PageId, ReplicationId LatestReplicationId, ReplicationId RequestedFromReplicationId, byte[] Events) {
    public PageId PageId { get; } = PageId;
    public ReplicationId LatestReplicationId { get; } = LatestReplicationId;
    public ReplicationId RequestedFromReplicationId { get; } = RequestedFromReplicationId;
    public byte[] Events { get; } = Events;
}

public interface IEventClient {
    Task<byte[]> FullPullAsync(PageId pageId);
    Task<PullData> PullAsync(PageId pageId, ReplicationId fromReplicationId);
    Task<ReplicationId> PushAsync(PageId pageId, ReplicationId latestReplicationId, byte[] data);
    Task<ReplicationId> ForcePushAsync(PageId pageId, byte[] data);
}
