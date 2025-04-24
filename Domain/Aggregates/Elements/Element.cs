using Domain.Aggregates;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;

namespace Domain.Aggregates.Elements;

public abstract class Element : AggregateRoot {
    public ElementId Id { get; }
    public BookId BookId { get; protected set; }
    public PageId PageId { get; protected set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; protected set; }

    protected Element(ElementId id, BookId bookId, PageId pageId, DateTime createdAt, DateTime updatedAt) {
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));

        Id = id;
        BookId = bookId;
        PageId = pageId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
