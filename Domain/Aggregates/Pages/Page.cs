using Domain.Aggregates.Books;
using System.Drawing;

namespace Domain.Aggregates.Pages;

public sealed class Page : AggregateRoot {
    public PageId Id { get; }
    public BookId BookId { get; private set; }
    public SizeF Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; private set; }

    private Page(PageId id, BookId bookId, SizeF size, DateTime createdAt, DateTime updatedAt) {
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));

        Id = id;
        BookId = bookId;
        Size = size;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Page Create(PageId id, BookId bookId, SizeF size, DateTime createdAt) {
        var element = new Page(id, bookId, size, createdAt, createdAt);

        // Page created event can be added here if needed

        return element;
    }
    public static Page Load(PageId id, BookId bookId, SizeF size, DateTime createdAt, DateTime updatedAt) {
        return new Page(id, bookId, size, createdAt, updatedAt);
    }
}
