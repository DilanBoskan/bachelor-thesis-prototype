using Application.Extensions.Serializer.Books;
using Application.Extensions.Serializer.Elements;
using Application.Extensions.Serializer.Pages;
using Domain.Events;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions.Serializer.Events;
public static class ElementRemovedFromPageEventSerializer {
    public static ElementRemovedFromPageEvent ToDomain(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.ElementRemovedFromPage) throw new ArgumentException("Invalid proto type", nameof(proto));

        var occurredAt = proto.OccurredAt.ToDateTime();
        var pageId = PageIdSerializer.ToDomain(proto.ElementRemovedFromPage.PageId);
        var elementId = ElementIdSerializer.ToDomain(proto.ElementRemovedFromPage.ElementId);

        return new ElementRemovedFromPageEvent(occurredAt, pageId, elementId);
    }

    public static Protos.Events.Event ToProto(ElementRemovedFromPageEvent value) {
        var occurredAt = value.OccurredAt.ToTimestamp();
        var pageId = PageIdSerializer.ToProto(value.PageId);
        var elementId = ElementIdSerializer.ToProto(value.ElementId);

        var @event = new Protos.Events.ElementRemovedFromPageEvent() {
            PageId = pageId,
            ElementId = elementId
        };

        return new Protos.Events.Event() {
            OccurredAt = occurredAt,
            ElementRemovedFromPage = @event
        };
    }
}
