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
public static class ElementAddedToPageEventSerializer {
    public static InkStrokeElementAddedToPageEvent ToDomain(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.ElementAddedToPage) throw new ArgumentException("Invalid proto type", nameof(proto));

        var occurredAt = proto.OccurredAt.ToDateTime();
        var pageId = PageIdSerializer.ToDomain(proto.ElementAddedToPage.PageId);
        var element = ElementSerializer.ToDomain(proto.ElementAddedToPage.Element);

        return new InkStrokeElementAddedToPageEvent(occurredAt, pageId, element);
    }

    public static Protos.Events.Event ToProto(InkStrokeElementAddedToPageEvent value) {
        var occurredAt = value.OccurredAt.ToTimestamp();
        var pageId = PageIdSerializer.ToProto(value.PageId);
        var element = ElementSerializer.ToProto(value.Element);

        var @event = new Protos.Events.ElementAddedToPageEvent() {
            PageId = pageId,
            Element = element
        };

        return new Protos.Events.Event() {
            OccurredAt = occurredAt,
            ElementAddedToPage = @event
        };
    }
}
