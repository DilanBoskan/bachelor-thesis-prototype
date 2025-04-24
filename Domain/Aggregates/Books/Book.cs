namespace Domain.Aggregates.Books;

public sealed class Book : AggregateRoot {
    public BookId Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }

    private Book(BookId id, DateTime createdAt, DateTime updatedAt) {
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));

        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Book Create(BookId id, DateTime createdAt) {
        var book = new Book(id, createdAt, createdAt);

        // Book created event can be added here if needed

        return book;
    }

    public static Book Load(BookId id, DateTime createdAt, DateTime updatedAt) {
        return new Book(id, createdAt, updatedAt);
    }
}
