using Domain.Events;

namespace Domain.Aggregates.Common;

public abstract class AggregateRoot<TSelf> : IAggregateRoot where TSelf : AggregateRoot<TSelf> {
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(IEvent @event) {
        _domainEvents.Add(@event);
    }
    public IReadOnlyCollection<IEvent> PopDomainEvents() {
        IReadOnlyCollection<IEvent> events = _domainEvents.ToList();
        _domainEvents.Clear();

        return events;
    }

    private readonly List<IEvent> _domainEvents = [];
}
