using Domain.Common;
using Domain.Entities.Elements;
using Domain.Entities.Pages;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages;

public record ElementDeletedMessage(DateTime TimeGenerated, PageId PageId, ElementId ElementId) : Message(TimeGenerated), IProtoSerializeable<ElementDeletedMessage, Protos.Messages.Message> {
    public static new ElementDeletedMessage FromProto(Protos.Messages.Message proto) {
        if (proto.MessageCase != Protos.Messages.Message.MessageOneofCase.ElementDeleted) throw new ArgumentException("Invalid proto type", nameof(proto));

        var timeGenerated = proto.TimeGenerated.ToDateTime();
        var pageId = PageId.FromProto(proto.ElementDeleted.PageId);
        var element = ElementId.FromProto(proto.ElementDeleted.ElementId);

        return new ElementDeletedMessage(
            timeGenerated,
            pageId,
            element
        );
    }

    public static Protos.Messages.Message ToProto(ElementDeletedMessage value) {
        var timeGeneratedProto = value.TimeGenerated.ToUniversalTime().ToTimestamp();
        var pageIdProto = PageId.ToProto(value.PageId);
        var elementIdProto = ElementId.ToProto(value.ElementId);

        return new Protos.Messages.Message() {
            TimeGenerated = timeGeneratedProto,
            ElementDeleted = new Protos.Messages.ElementDeletedMessage() {
                PageId = pageIdProto,
                ElementId = elementIdProto
            }
        };
    }
}
