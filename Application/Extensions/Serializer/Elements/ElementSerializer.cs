using Application.Protos;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions.Serializer.Elements;

public static class ElementSerializer {
    public static Element ToDomain(Protos.Elements.Element proto) {
        return proto.ElementCase switch {
            Protos.Elements.Element.ElementOneofCase.InkStroke => InkStrokeElementSerializer.ToDomain(proto),
            _ => throw new NotImplementedException()
        };
    }

    public static Protos.Elements.Element ToProto(Element value) {
        return value switch {
            InkStrokeElement inkStroke => InkStrokeElementSerializer.ToProto(inkStroke),
            _ => throw new NotImplementedException()
        };
    }
}
