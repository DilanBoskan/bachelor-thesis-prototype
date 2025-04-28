using Domain.Aggregates.Elements;
using Domain.Aggregates.Pages;

namespace Domain.Events;

public record ElementRemovedFromPageEvent(DateTime OccurredAt, PageId PageId, ReplicationId ReplicationId, ElementId ElementId) : Event(OccurredAt, PageId, ReplicationId);