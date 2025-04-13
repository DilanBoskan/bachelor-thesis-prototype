namespace Domain.Entities.Books;

public sealed record BookId(Guid Value) {
    public static BookId New() => new(Guid.NewGuid());
    public static BookId Empty() => new(Guid.Empty);
    public bool IsEmpty() => Value == Guid.Empty;
}
