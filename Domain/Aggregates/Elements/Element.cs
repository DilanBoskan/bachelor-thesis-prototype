using Domain.Aggregates;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;

namespace Domain.Aggregates.Elements;

public abstract class Element : AggregateRoot {
    public ElementId Id { get; protected set; } = null!;
    public BookId BookId { get; protected set; } = null!;
    public PageId PageId { get; protected set; } = null!;
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }

    protected Element() { } // For serialization
}
