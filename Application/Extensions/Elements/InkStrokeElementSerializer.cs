using Application.Extensions.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System.Numerics;

namespace Application.Extensions.Elements;

public static class InkStrokeElementSerializer {
    public static InkStrokeElement ToDomain(Protos.Elements.Element proto) {
        if (proto.ElementCase != Protos.Elements.Element.ElementOneofCase.InkStroke) throw new ArgumentException("Invalid proto type", nameof(proto));

        var id = ElementIdSerializer.ToDomain(proto.Id);
        var creationDate = proto.CreationDate.ToDateTime();
        var inkPoints = proto.InkStroke.Points
            .Select(p => new InkStrokePoint(new Vector2(p.PositionX, p.PositionY), p.Pressure))
            .ToArray();

        return new InkStrokeElement(
            id,
            creationDate,
            inkPoints
        );
    }
    public static Protos.Elements.Element ToProto(InkStrokeElement value) {
        var id = ElementIdSerializer.ToProto(value.Id);
        var creationDate = value.CreationDate.ToUniversalTime().ToTimestamp();
        var inkPoints = value.Points
            .Select(v => new Protos.Elements.InkStrokePoint() {
                PositionX = v.Position.X,
                PositionY = v.Position.Y,
                Pressure = v.Pressure
            });

        var inkStrokeProto = new Protos.Elements.InkStrokeElement();
        inkStrokeProto.Points.AddRange(inkPoints);

        return new Protos.Elements.Element() {
            Id = id,
            CreationDate = creationDate,
            InkStroke = inkStrokeProto,
        };
    }
}