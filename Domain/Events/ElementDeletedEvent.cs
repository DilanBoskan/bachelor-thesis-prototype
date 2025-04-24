using Domain.Aggregates;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;

public record ElementDeletedEvent(BookId BookId, PageId PageId, ElementId ElementId) : IEvent {
    public UserId UserId { get; set; } = null!;
}