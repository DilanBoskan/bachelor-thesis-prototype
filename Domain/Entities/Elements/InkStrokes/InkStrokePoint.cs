using Domain.Common;
using System.Numerics;

namespace Domain.Entities.Elements.InkStrokes;

public readonly record struct InkStrokePoint(Vector2 Position, float Pressure = 0.5f) : IProtoSerializeable<IReadOnlyList<InkStrokePoint>, IReadOnlyList<Domain.Protos.Elements.InkStrokePoint>> {
    public static IReadOnlyList<InkStrokePoint> FromProto(IReadOnlyList<Protos.Elements.InkStrokePoint> proto) {
        return proto
            .Select(p => new InkStrokePoint(new Vector2(p.PositionX, p.PositionY), p.Pressure))
            .ToList();
    }

    public static IReadOnlyList<Protos.Elements.InkStrokePoint> ToProto(IReadOnlyList<InkStrokePoint> value) {
        return value
            .Select(v => new Protos.Elements.InkStrokePoint() {
                PositionX = v.Position.X,
                PositionY = v.Position.Y,
                Pressure = v.Pressure
            })
            .ToList();
    }
}
