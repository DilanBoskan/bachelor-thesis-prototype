using Application.Extensions.Serializer.Elements;
using Application.Extensions.Serializer.Pages;
using Domain.Events;
using Google.Protobuf.WellKnownTypes;

namespace Application.Extensions.Serializer.Events;
public static class ElementRemovedFromPageEventSerializer {
    public static ElementRemovedFromPageEvent ToDomain(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.ElementRemovedFromPage) throw new ArgumentException("Invalid proto type", nameof(proto));

        var occurredAt = proto.OccurredAt.ToDateTime();
        var pageId = PageIdSerializer.ToDomain(proto.PageId);
        var replicationId = ReplicationIdSerializer.ToDomain(proto.ReplicationId);
        var elementId = ElementIdSerializer.ToDomain(proto.ElementRemovedFromPage.ElementId);

        return new ElementRemovedFromPageEvent(occurredAt, pageId, replicationId, elementId);
    }

    public static Protos.Events.Event ToProto(ElementRemovedFromPageEvent value) {
        var occurredAt = value.OccurredAt.ToTimestamp();
        var pageId = PageIdSerializer.ToProto(value.PageId);
        var replicationId = ReplicationIdSerializer.ToProto(value.ReplicationId);
        var elementId = ElementIdSerializer.ToProto(value.ElementId);

        var @event = new Protos.Events.ElementRemovedFromPageEvent() {
            ElementId = elementId
        };

        return new Protos.Events.Event() {
            OccurredAt = occurredAt,
            PageId = pageId,
            ReplicationId = replicationId,
            ElementRemovedFromPage = @event
        };
    }
}
