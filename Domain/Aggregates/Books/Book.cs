using Domain.Aggregates.Common;
using Domain.Aggregates.Pages;
using System.Drawing;

namespace Domain.Aggregates.Books;

public sealed class Book : AggregateRoot {
    public BookId Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyList<Book.Page> Pages => _pages;

    public Book(BookId id, DateTime createdAt, DateTime updatedAt, IEnumerable<Book.Page> pages) {
        ArgumentNullException.ThrowIfNull(id);
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));
        ArgumentNullException.ThrowIfNull(pages);

        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _pages = pages.ToList();
    }

    private readonly List<Book.Page> _pages;

    public record Page(PageId Id, SizeF Size, DateTime CreatedAt, DateTime UpdatedAt);
}