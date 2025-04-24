using Domain.Aggregates;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;

public record ElementCreatedEvent(Element Element) : IEvent {
    public UserId UserId { get; set; } = null!;
}