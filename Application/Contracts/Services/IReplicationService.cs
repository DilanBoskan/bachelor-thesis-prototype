using Domain.Aggregates.Pages;
using Domain.Events;

namespace Application.Contracts.Services;

public interface IReplicationService {
    Task<IReadOnlyList<IEvent>> PullAsync(PageId pageId);
    Task PushAsync();
}