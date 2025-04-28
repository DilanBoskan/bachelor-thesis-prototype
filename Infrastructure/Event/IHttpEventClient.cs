using Application.Contracts.Event;
using Domain.Aggregates.Pages;
using Refit;

namespace Infrastructure.Event;

public interface IHttpEventClient {
    [Get("/pages/{pageId}/_replication/full")]
    Task<byte[]> FullPullAsync(PageId pageId);

    [Get("/pages/{pageId}/_replication")]
    Task<PullData> PullAsync(PageId pageId, [Header("X-Replication-Id")] ReplicationId fromReplicationId);

    [Patch("/pages/{pageId}/_replication")]
    Task<ReplicationId> PushAsync(PageId pageId, [Header("X-Replication-Id")] ReplicationId latestReplicationId, [Body] byte[] data);

    [Post("/pages/{pageId}/_replication")]
    Task<ReplicationId> ForcePushAsync(PageId pageId, [Body] byte[] data);
}
