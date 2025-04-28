using Domain.Aggregates.Common;
using Domain.Aggregates.Pages;
using System.Drawing;

namespace Domain.Aggregates.Books;


public record BookPage(PageId Id, SizeF Size, DateTime CreatedAt, DateTime UpdatedAt);

public sealed class Book : AggregateRoot {
    public BookId Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyList<BookPage> Pages => _pages;

    public Book(BookId id, DateTime createdAt, DateTime updatedAt, IEnumerable<BookPage> pages) {
        ArgumentNullException.ThrowIfNull(id);
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));
        ArgumentNullException.ThrowIfNull(pages);

        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _pages = pages.ToList();
    }

    private readonly List<BookPage> _pages;
}