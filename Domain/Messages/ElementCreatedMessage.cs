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

public record ElementCreatedMessage(DateTime TimeGenerated, PageId PageId, Element Element) : Message(TimeGenerated), IProtoSerializeable<ElementCreatedMessage, Protos.Messages.Message> {
    public static new ElementCreatedMessage FromProto(Protos.Messages.Message proto) {
        if (proto.MessageCase != Protos.Messages.Message.MessageOneofCase.ElementCreated) throw new ArgumentException("Invalid proto type", nameof(proto));

        var timeGenerated = proto.TimeGenerated.ToDateTime();
        var pageId = PageId.FromProto(proto.ElementCreated.PageId);
        var element = Element.FromProto(proto.ElementCreated.Element);

        return new ElementCreatedMessage(
            timeGenerated,
            pageId,
            element
        );
    }

    public static Protos.Messages.Message ToProto(ElementCreatedMessage value) {
        var timeGeneratedProto = value.TimeGenerated.ToUniversalTime().ToTimestamp();
        var pageIdProto = PageId.ToProto(value.PageId);
        var elementProto = Element.ToProto(value.Element);

        return new Protos.Messages.Message() {
            TimeGenerated = timeGeneratedProto,
            ElementCreated = new Protos.Messages.ElementCreatedMessage() {
                PageId = pageIdProto,
                Element = elementProto
            }
        };
    }
}
