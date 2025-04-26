using Application.Contracts.Services;
using Domain.Aggregates.Pages;
using Domain.Events;

namespace Application.Features.Merge;
public class MergeService : IMergeService {
    public void Merge(Page page, IReadOnlyList<IEvent> events) {
        foreach (var @event in events) {
            page.Apply(@event);
        }
    }
}
