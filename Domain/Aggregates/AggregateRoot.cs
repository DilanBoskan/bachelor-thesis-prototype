using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates;

public abstract class AggregateRoot {
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
