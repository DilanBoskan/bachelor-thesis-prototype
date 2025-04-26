using Domain.Aggregates.Elements;
using System;

namespace Presentation.Models.Elements;
public abstract class ElementModel(ElementId id, DateTime createdAt, DateTime updatedAt) : ObservableObjectWithResources {
    public ElementId Id { get; } = id;
    public DateTime CreatedAt { get; } = createdAt;
    public DateTime UpdatedAt { get; } = updatedAt;
}
