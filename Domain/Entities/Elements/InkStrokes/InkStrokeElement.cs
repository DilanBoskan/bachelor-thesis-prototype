namespace Domain.Entities.Elements.InkStrokes;

public sealed record InkStrokeElement(ElementId Id, DateTime CreationDate, IReadOnlyList<InkStrokePoint> Points) : Element(Id, CreationDate);
