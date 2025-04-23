namespace Domain.Aggregates.Elements;

public sealed record ElementId(Guid Value) : BaseId<ElementId>(Value), IBaseId<ElementId> {
    public static ElementId New() => new(Guid.NewGuid());
    public static ElementId Create(Guid value) => new(value);
}
