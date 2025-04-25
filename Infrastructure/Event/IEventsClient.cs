using Application.Contracts.Replication;
using Domain.Aggregates.Books;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event;

public record PullData(ReplicationVersion LatestReplicationId, ReplicationVersion RequestedFromReplicationId, IReadOnlyList<(ReplicationVersion ReplicationId, byte[] Data)> Events);

public interface IEventsClient {
    [Get("/books/{bookId}/events")]
    Task<PullData> PullAsync(BookId bookId, [Header("X-Replication-Id")] ReplicationVersion fromReplicationId);

    [Post("/books/{bookId}/events")]
    Task<ReplicationVersion> PushAsync(BookId bookId, [Header("X-Replication-Id")] ReplicationVersion latestReplicationId, [Body] byte[] data);
}
