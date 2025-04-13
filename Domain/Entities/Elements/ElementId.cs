namespace Domain.Entities.Elements;

public sealed record ElementId(Guid Value) {
    public static ElementId New() => new(Guid.NewGuid());
    public static ElementId Empty() => new(Guid.Empty);
    public bool IsEmpty() => Value == Guid.Empty;
}
