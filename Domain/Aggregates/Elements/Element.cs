using Domain.Aggregates.Books;
using Domain.Aggregates.Common;

namespace Domain.Aggregates.Elements;

public abstract class Element : IEntity {
    public ElementId Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; protected set; }

    protected Element(ElementId id, DateTime createdAt, DateTime updatedAt) {
        if (createdAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(createdAt));
        if (updatedAt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be of Kind Utc", nameof(updatedAt));

        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
