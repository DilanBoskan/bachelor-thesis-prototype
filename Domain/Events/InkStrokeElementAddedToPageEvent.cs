using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;

public record InkStrokeElementAddedToPageEvent(DateTime OccurredAt, PageId PageId, ElementId ElementId, DateTime CreatedAt, IReadOnlyList<InkStrokePoint> Points) : IEvent;