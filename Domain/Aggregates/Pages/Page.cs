using Domain.Aggregates.Common;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Events;
using System.Drawing;

namespace Domain.Aggregates.Pages;

public sealed class Page : AggregateRoot<Page>, IApplyEvent {
    public PageId Id { get; }
    public ReplicationId ReplicationId { get; private set; }
    public SizeF Size { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyList<Element> Elements => _elements;

    public Page(PageId id, ReplicationId replicationId, SizeF size, DateTime createdAt, DateTime updatedAt, IEnumerable<Element> elements) {
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));

        Id = id;
        ReplicationId = replicationId;
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
        ReplicationId = ReplicationId.Next();

        AddDomainEvent(new InkStrokeElementAddedToPageEvent(UpdatedAt, Id, ReplicationId, element.Id, createdAt, points));

        return element;
    }
    public void RemoveElement(ElementId elementId) {
        ArgumentNullException.ThrowIfNull(elementId);
        if (!_elements.Any(e => e.Id == elementId)) throw new InvalidOperationException($"Element {elementId} does not exist on this page.");

        _elements.RemoveAll(e => e.Id == elementId);
        UpdatedAt = DateTime.UtcNow;
        ReplicationId = ReplicationId.Next();

        AddDomainEvent(new ElementRemovedFromPageEvent(UpdatedAt, Id, ReplicationId, elementId));
    }

    #region Events
    public void Apply(IEvent @event) {
        switch (@event) {
            case InkStrokeElementAddedToPageEvent inkStrokeElementAddedToPageEvent:
                Apply(inkStrokeElementAddedToPageEvent);
                break;
            case ElementRemovedFromPageEvent elementRemovedFromPageEvent:
                Apply(elementRemovedFromPageEvent);
                break;
            default:
                throw new NotImplementedException();
        }
    }
    public void Apply(InkStrokeElementAddedToPageEvent @event) {
        var element = @event.ToInkStrokeElement();

        _elements.Add(element);
        UpdatedAt = @event.OccurredAt;
        ReplicationId = @event.ReplicationId;
    }
    public void Apply(ElementRemovedFromPageEvent @event) {
        _elements.RemoveAll(e => e.Id == @event.ElementId);
        UpdatedAt = @event.OccurredAt;
        ReplicationId = @event.ReplicationId;
    }
    #endregion

    private readonly List<Element> _elements;
}
