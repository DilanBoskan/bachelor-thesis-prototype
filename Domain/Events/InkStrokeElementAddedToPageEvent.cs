using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;

namespace Domain.Events;

public record InkStrokeElementAddedToPageEvent(DateTime OccurredAt, PageId PageId, ReplicationId ReplicationId, ElementId ElementId, DateTime CreatedAt, IReadOnlyList<InkStrokePoint> Points) : Event(OccurredAt, PageId, ReplicationId) {
    public InkStrokeElement ToInkStrokeElement() => new(ElementId, CreatedAt, CreatedAt, Points);
}