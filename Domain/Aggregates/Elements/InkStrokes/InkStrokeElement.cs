using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using Domain.Events;
using System.Numerics;

namespace Domain.Aggregates.Elements.InkStrokes;

public sealed class InkStrokeElement : Element {
    public IReadOnlyList<InkStrokePoint> Points { get; private set; }

    private InkStrokeElement(ElementId id, BookId bookId, PageId pageId, DateTime createdAt, DateTime updatedAt, IReadOnlyList<InkStrokePoint> points) : base(id, bookId, pageId, createdAt, updatedAt) {
        Points = points;
    } 

    public static InkStrokeElement Create(ElementId id, BookId bookId, PageId pageId, DateTime createdAt, IReadOnlyList<InkStrokePoint> points) {
        var element = new InkStrokeElement(id, bookId, pageId, createdAt, createdAt, points);

        element.AddDomainEvent(new ElementCreatedEvent(element));

        return element;
    }
    public static InkStrokeElement Load(ElementId id, BookId bookId, PageId pageId, DateTime createdAt, DateTime updatedAt, IReadOnlyList<InkStrokePoint> points) {
        return new InkStrokeElement(id, bookId, pageId, createdAt, updatedAt, points);
    }

    public static InkStrokeElement CreateRandom(BookId bookId, PageId pageId) {
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

        return Create(ElementId.New(), bookId, pageId, DateTime.UtcNow, points);
    }
}
