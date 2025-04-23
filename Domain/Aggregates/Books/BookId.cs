namespace Domain.Aggregates.Books;

public sealed record BookId(Guid Value) : BaseId<BookId>(Value), IBaseId<BookId> {
    public static BookId New() => new(Guid.NewGuid());
    public static BookId Create(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}