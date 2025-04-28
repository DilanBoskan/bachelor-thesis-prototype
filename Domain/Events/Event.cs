using Domain.Aggregates.Pages;

namespace Domain.Events;

public abstract record Event(DateTime OccurredAt, PageId PageId, ReplicationId ReplicationId);
