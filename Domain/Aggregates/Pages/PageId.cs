namespace Domain.Aggregates.Pages;

public sealed record PageId(Guid Value) : BaseId<PageId>(Value), IBaseId<PageId> {
    public static PageId New() => new(Guid.NewGuid());
    public static PageId Create(Guid value) => new(value);
}