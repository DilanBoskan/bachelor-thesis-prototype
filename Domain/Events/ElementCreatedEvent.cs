using Domain.Common;
using Domain.Entities.Elements;
using Domain.Entities.Pages;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;

public record ElementCreatedEvent(DateTime TimeGenerated, PageId PageId, Element Element) : Event(TimeGenerated), IProtoSerializeable<ElementCreatedEvent, Protos.Events.Event> {
    public static new ElementCreatedEvent FromProto(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.ElementCreated) throw new ArgumentException("Invalid proto type", nameof(proto));

        var timeGenerated = proto.TimeGenerated.ToDateTime();
        var pageId = PageId.FromProto(proto.ElementCreated.PageId);
        var element = Element.FromProto(proto.ElementCreated.Element);

        return new ElementCreatedEvent(
            timeGenerated,
            pageId,
            element
        );
    }

    public static Protos.Events.Event ToProto(ElementCreatedEvent value) {
        var timeGeneratedProto = value.TimeGenerated.ToUniversalTime().ToTimestamp();
        var pageIdProto = PageId.ToProto(value.PageId);
        var elementProto = Element.ToProto(value.Element);

        return new Protos.Events.Event() {
            TimeGenerated = timeGeneratedProto,
            ElementCreated = new Protos.Events.ElementCreatedEvent() {
                PageId = pageIdProto,
                Element = elementProto
            }
        };
    }
}
