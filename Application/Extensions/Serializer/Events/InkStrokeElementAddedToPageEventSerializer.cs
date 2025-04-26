using Application.Extensions.Serializer.Elements;
using Application.Extensions.Serializer.Pages;
using Domain.Events;
using Google.Protobuf.WellKnownTypes;

namespace Application.Extensions.Serializer.Events;
public static class InkStrokeElementAddedToPageEventSerializer {
    public static InkStrokeElementAddedToPageEvent ToDomain(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.InkStrokeElementAddedToPage) throw new ArgumentException("Invalid proto type", nameof(proto));

        var occurredAt = proto.OccurredAt.ToDateTime();
        var pageId = PageIdSerializer.ToDomain(proto.PageId);
        var replicationId = ReplicationIdSerializer.ToDomain(proto.ReplicationId);
        var elementId = ElementIdSerializer.ToDomain(proto.InkStrokeElementAddedToPage.ElementId);
        var createdAt = proto.InkStrokeElementAddedToPage.CreatedAt.ToDateTime();
        var points = InkStrokeElementSerializer.PointsToDomain(proto.InkStrokeElementAddedToPage.Points);

        return new InkStrokeElementAddedToPageEvent(occurredAt, pageId, replicationId, elementId, createdAt, points);
    }

    public static Protos.Events.Event ToProto(InkStrokeElementAddedToPageEvent value) {
        var occurredAt = value.OccurredAt.ToTimestamp();
        var pageId = PageIdSerializer.ToProto(value.PageId);
        var replicationId = ReplicationIdSerializer.ToProto(value.ReplicationId);
        var elementId = ElementIdSerializer.ToProto(value.ElementId);
        var createdAt = value.CreatedAt.ToTimestamp();
        var points = InkStrokeElementSerializer.PointsToProto(value.Points);

        var @event = new Protos.Events.InkStrokeElementAddedToPageEvent() {
            ElementId = elementId,
            CreatedAt = createdAt,
            Points = { points }
        };

        return new Protos.Events.Event() {
            OccurredAt = occurredAt,
            PageId = pageId,
            ReplicationId = replicationId,
            InkStrokeElementAddedToPage = @event
        };
    }
}
