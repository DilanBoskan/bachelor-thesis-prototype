using Domain.Aggregates.Pages;
using Domain.Events;

namespace Application.Contracts.Services;
public interface IMergeService {
    void Merge(Page page, IReadOnlyList<Domain.Events.Event> events);
}
