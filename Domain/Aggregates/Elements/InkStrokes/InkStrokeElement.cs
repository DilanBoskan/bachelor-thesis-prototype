using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Pages;
using Domain.Events;
using System.Numerics;

namespace Domain.Aggregates.Elements.InkStrokes;

public sealed class InkStrokeElement : Element {
    public IReadOnlyList<InkStrokePoint> Points { get; private set; }

    public InkStrokeElement(ElementId id, DateTime createdAt, DateTime updatedAt, IEnumerable<InkStrokePoint> points) : base(id, createdAt, updatedAt) {
        ArgumentNullException.ThrowIfNull(points);
        if (!points.Any()) throw new ArgumentException("Points cannot be empty", nameof(points));

        Points = points.ToList().AsReadOnly();
    }

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

        return new InkStrokeElement(ElementId.New(), DateTime.UtcNow, DateTime.UtcNow, points);
    }
}
