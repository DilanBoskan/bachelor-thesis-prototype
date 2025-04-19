using Domain.Common;
using Google.Protobuf.WellKnownTypes;
using System.Numerics;

namespace Domain.Entities.Elements.InkStrokes;

public sealed record InkStrokeElement(ElementId Id, DateTime CreationDate, IReadOnlyList<InkStrokePoint> Points) : Element(Id, CreationDate), IProtoSerializeable<InkStrokeElement, Protos.Elements.Element> {
    public static InkStrokeElement CreateRandom() {
        var random = new Random();
        var maxPoints = random.Next(100, 200);

        List<InkStrokePoint> points = new(maxPoints);

        var initialPoint = new Vector2(random.Next(50, 800), random.Next(50, 800));
        points.Add(new InkStrokePoint(initialPoint));
        for (var i = 1; i < maxPoints - 1; i++) {
            var prevPoint = points[i - 1];
            var translate = new Vector2(random.Next(-5, 10), random.Next(-5, 10));

            var newPoint = new InkStrokePoint(prevPoint.Position + translate);
            points.Add(newPoint);
        }

        return new InkStrokeElement(ElementId.New(), DateTime.Now, points);
    }

    public static new InkStrokeElement FromProto(Protos.Elements.Element proto) {
        if (proto.ElementCase != Protos.Elements.Element.ElementOneofCase.InkStroke) throw new ArgumentException("Invalid proto type", nameof(proto));

        var id = ElementId.FromProto(proto.Id);
        var creationDate = proto.CreationDate.ToDateTime();

        return new InkStrokeElement(
            id,
            creationDate,
            InkStrokePoint.FromProto(proto.InkStroke.Points)
        );
    }

    public static Protos.Elements.Element ToProto(InkStrokeElement value) {
        var id = ElementId.ToProto(value.Id);
        var creationDate = value.CreationDate.ToTimestamp();

        var inkStrokeProto = new Protos.Elements.InkStrokeElement();
        inkStrokeProto.Points.AddRange(InkStrokePoint.ToProto(value.Points));

        return new Protos.Elements.Element() {
            Id = id,
            CreationDate = creationDate,
            InkStroke = inkStrokeProto,
        };
    }
}
