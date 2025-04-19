using Domain.Common;
using Domain.Entities.Elements.InkStrokes;
using Google.Protobuf.WellKnownTypes;

namespace Domain.Entities.Elements;

public abstract record Element(ElementId Id, DateTime CreationDate) : IProtoSerializeable<Element, Domain.Protos.Elements.Element> {
    public static Protos.Elements.Element ToProto(Element value) {
        return value switch {
            InkStrokeElement inkStroke => InkStrokeElement.ToProto(inkStroke),
            _ => throw new NotImplementedException()
        };
    }
    public static Element FromProto(Domain.Protos.Elements.Element proto) {
        return proto.ElementCase switch {
            Domain.Protos.Elements.Element.ElementOneofCase.InkStroke => InkStrokeElement.FromProto(proto),
            _ => throw new NotImplementedException()
        };
    }
}
