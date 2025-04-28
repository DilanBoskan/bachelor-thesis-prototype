using Domain.Events;

namespace Domain.Aggregates.Common;

public abstract class AggregateRoot : IAggregateRoot {
    public IReadOnlyList<Event> DomainEvents => _domainEvents;

    protected void AddDomainEvent(Event @event) {
        _domainEvents.Add(@event);
    }
    public IReadOnlyCollection<Event> PopDomainEvents() {
        IReadOnlyCollection<Event> events = _domainEvents.ToList();
        _domainEvents.Clear();

        return events;
    }

    private readonly List<Event> _domainEvents = [];
}
