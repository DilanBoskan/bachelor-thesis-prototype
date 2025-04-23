using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models.Elements;
public abstract class ElementModel(BookId bookId, PageId pageId, ElementId id, DateTime createdAt, DateTime updatedAt) : ObservableObjectWithResources {
    public ElementId Id { get; } = id;
    public BookId BookId { get; } = bookId;
    public PageId PageId { get; } = pageId;
    public DateTime CreatedAt { get; } = createdAt;
    public DateTime UpdatedAt { get; } = updatedAt;
}
