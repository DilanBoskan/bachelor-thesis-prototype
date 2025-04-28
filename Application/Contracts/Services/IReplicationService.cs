using Domain.Aggregates.Pages;
using Domain.Events;

namespace Application.Contracts.Services;

public interface IReplicationService {
    Task<IReadOnlyList<Domain.Events.Event>> PullAsync(PageId pageId);
    Task PushAsync();
}