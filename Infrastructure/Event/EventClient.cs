using Application.Contracts.Event;
using Domain.Aggregates.Pages;

namespace Infrastructure.Event;
public class EventClient(IHttpEventClient httpEventClient) : IEventClient {
    private readonly IHttpEventClient _httpEventClient = httpEventClient;

    public Task<byte[]> FullPullAsync(PageId pageId) => _httpEventClient.FullPullAsync(pageId);
    public Task<PullData> PullAsync(PageId pageId, ReplicationId fromReplicationId) => _httpEventClient.PullAsync(pageId, fromReplicationId);
    public Task<ReplicationId> PushAsync(PageId pageId, ReplicationId latestReplicationId, byte[] data) => _httpEventClient.PushAsync(pageId, latestReplicationId, data);
    public Task<ReplicationId> ForcePushAsync(PageId pageId, byte[] data) => _httpEventClient.ForcePushAsync(pageId, data);
}
