namespace Domain.Entities.Pages;
public sealed record PageId(Guid Value) {
    public static PageId Create(Guid value) => new(value);
    public static PageId New() => new(Guid.NewGuid());
}