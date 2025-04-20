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

public record ElementDeletedEvent(DateTime TimeGenerated, PageId PageId, ElementId ElementId) : Event(TimeGenerated), IProtoSerializeable<ElementDeletedEvent, Protos.Events.Event> {
    public static new ElementDeletedEvent FromProto(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.ElementDeleted) throw new ArgumentException("Invalid proto type", nameof(proto));

        var timeGenerated = proto.TimeGenerated.ToDateTime();
        var pageId = PageId.FromProto(proto.ElementDeleted.PageId);
        var element = ElementId.FromProto(proto.ElementDeleted.ElementId);

        return new ElementDeletedEvent(
            timeGenerated,
            pageId,
            element
        );
    }

    public static Protos.Events.Event ToProto(ElementDeletedEvent value) {
        var timeGeneratedProto = value.TimeGenerated.ToUniversalTime().ToTimestamp();
        var pageIdProto = PageId.ToProto(value.PageId);
        var elementIdProto = ElementId.ToProto(value.ElementId);

        return new Protos.Events.Event() {
            TimeGenerated = timeGeneratedProto,
            ElementDeleted = new Protos.Events.ElementDeletedEvent() {
                PageId = pageIdProto,
                ElementId = elementIdProto
            }
        };
    }
}
