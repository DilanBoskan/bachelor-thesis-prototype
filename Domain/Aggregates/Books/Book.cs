using Domain.Aggregates.Common;
using Domain.Aggregates.Pages;

namespace Domain.Aggregates.Books;

public sealed class Book : AggregateRoot {
    public BookId Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyList<PageId> Pages => _pages;

    public Book(BookId id, DateTime createdAt, DateTime updatedAt, IEnumerable<PageId> pages) {
        ArgumentNullException.ThrowIfNull(id);
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));
        ArgumentNullException.ThrowIfNull(pages);

        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _pages = pages.ToList();
    }

    private readonly List<PageId> _pages;
}