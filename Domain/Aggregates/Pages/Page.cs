using Domain.Aggregates.Books;
using Domain.Aggregates.Common;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Events;
using System.Drawing;
using System.Reflection;

namespace Domain.Aggregates.Pages;

public sealed class Page : AggregateRoot, IApplyEvent<InkStrokeElementAddedToPageEvent>, IApplyEvent<ElementRemovedFromPageEvent> {
    public PageId Id { get; }
    public SizeF Size { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyList<Element> Elements => _elements;

    public Page(PageId id, SizeF size, DateTime createdAt, DateTime updatedAt, IEnumerable<Element> elements) {
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));

        Id = id;
        Size = size;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _elements = elements.ToList();
    }

    public InkStrokeElement CreateInkStrokeElement(DateTime createdAt, IReadOnlyList<InkStrokePoint> points) {
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (createdAt > DateTime.UtcNow) throw new ArgumentException("DateTime cannot be in the future", nameof(createdAt));

        var element = new InkStrokeElement(ElementId.New(), createdAt, createdAt, points);

        _elements.Add(element);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new InkStrokeElementAddedToPageEvent(UpdatedAt, Id, element.Id, createdAt, points));

        return element;
    }
    public void Apply(InkStrokeElementAddedToPageEvent @event) {
        var element = new InkStrokeElement(@event.ElementId, @event.CreatedAt, @event.CreatedAt, @event.Points);

        _elements.Add(element);
        UpdatedAt = @event.OccurredAt;
    }

    public void RemoveElement(ElementId elementId) {
        ArgumentNullException.ThrowIfNull(elementId);
        if (!_elements.Any(e => e.Id == elementId)) throw new InvalidOperationException($"Element {elementId} does not exist on this page.");

        _elements.RemoveAll(e => e.Id == elementId);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ElementRemovedFromPageEvent(UpdatedAt, Id, elementId));
    }
    public void Apply(ElementRemovedFromPageEvent @event) {
        _elements.RemoveAll(e => e.Id == @event.ElementId);
        UpdatedAt = @event.OccurredAt;
    }


    private readonly List<Element> _elements;
}
