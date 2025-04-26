using Domain.Aggregates.Elements.InkStrokes;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using System.Numerics;

namespace Application.Extensions.Serializer.Elements;

public static class InkStrokeElementSerializer {
    public static InkStrokeElement ToDomain(Protos.Elements.Element proto) {
        if (proto.ElementCase != Protos.Elements.Element.ElementOneofCase.InkStroke) throw new ArgumentException("Invalid proto type", nameof(proto));

        var id = ElementIdSerializer.ToDomain(proto.Id);
        var createdAt = proto.CreatedAt.ToDateTime();
        var updatedAt = proto.UpdatedAt.ToDateTime();
        var inkPoints = PointsToDomain(proto.InkStroke.Points);

        return new InkStrokeElement(
            id,
            createdAt,
            updatedAt,
            inkPoints
        );
    }

    public static Protos.Elements.Element ToProto(InkStrokeElement value) {
        var id = ElementIdSerializer.ToProto(value.Id);
        var createdAt = value.CreatedAt.ToTimestamp();
        var updatedAt = value.UpdatedAt.ToTimestamp();
        var inkPoints = PointsToProto(value.Points);

        var inkStrokeProto = new Protos.Elements.InkStrokeElement();
        inkStrokeProto.Points.AddRange(inkPoints);

        return new Protos.Elements.Element() {
            Id = id,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            InkStroke = inkStrokeProto,
        };
    }

    public static IReadOnlyList<InkStrokePoint> PointsToDomain(RepeatedField<Protos.Elements.InkStrokePoint> protos) {
        return protos
            .Select(p => new InkStrokePoint(new Vector2(p.PositionX, p.PositionY), p.Pressure))
            .ToArray();
    }
    public static IEnumerable<Protos.Elements.InkStrokePoint> PointsToProto(IReadOnlyList<InkStrokePoint> values) {
        return values
            .Select(v => new Protos.Elements.InkStrokePoint() {
                PositionX = v.Position.X,
                PositionY = v.Position.Y,
                Pressure = v.Pressure
            });
    }
}