using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Domain.Events;
using Presentation.Events;
using Presentation.Events.Pages;
using Presentation.Models.Books;
using Presentation.Models.Elements;
using Presentation.Models.Elements.InkStrokes;
using Presentation.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace Presentation.Extensions;
public static class MappingExtensions {
    public static BookModel ToWindows(this Book book) => new(book);
    public static Book ToDomain(this BookModel book) => Book.Load(book.Id, book.CreatedAt, book.UpdatedAt);

    public static PageModel ToWindows(this Page page) => new(page);
    public static Page ToDomain(this PageModel page) => Page.Load(page.Id, page.BookId, page.Size, page.CreatedAt, page.UpdatedAt);


    public static ElementModel ToWindows(this Element element) => element switch {
        InkStrokeElement inkStrokeElement => inkStrokeElement.ToWindows(),
        _ => throw new NotImplementedException($"Mapping for {element.GetType()} is not implemented")
    };
    public static Element ToDomain(this ElementModel element) => element switch {
        InkStrokeElementModel inkStrokeElement => inkStrokeElement.ToDomain(),
        _ => throw new NotImplementedException($"Mapping for {element.GetType()} is not implemented")
    };

    public static InkStrokeElementModel ToWindows(this InkStrokeElement inkStrokeElement) => new(inkStrokeElement);
    public static InkStrokeElement ToDomain(this InkStrokeElementModel inkStrokeElement) => InkStrokeElement.Load(inkStrokeElement.Id, inkStrokeElement.BookId, inkStrokeElement.PageId, inkStrokeElement.CreatedAt, inkStrokeElement.UpdatedAt, inkStrokeElement.InkStroke.ToInkStrokePoints());
    public static InkStroke ToInkStroke(this IEnumerable<InkStrokePoint> inkStrokePoints) {
        var builder = new InkStrokeBuilder();

        var points = inkStrokePoints
            .Select(p => new InkPoint(p.Position.ToPoint(), p.Pressure))
            .ToList();
        var stroke = builder.CreateStrokeFromInkPoints(points, Matrix3x2.Identity);

        return stroke;
    }
    public static InkStrokePoint[] ToInkStrokePoints(this InkStroke stroke) {
        var points = stroke.GetInkPoints()
            .Select(p => new InkStrokePoint(p.Position.ToVector2(), p.Pressure))
            .ToArray();

        return points;
    }

    #region Events
    public static IWindowsEvent ToWindows(this IEvent @event) {
        return @event switch {
            ElementCreatedEvent elementCreatedEvent => elementCreatedEvent.ToWindows(),
            ElementDeletedEvent elementDeletedEvent => elementDeletedEvent.ToWindows(),
            _ => throw new NotImplementedException($"Mapping for {@event.GetType()} is not implemented")
        };
    }
    public static IEvent ToDomain(this IWindowsEvent @event) {
        return @event switch {
            WindowsElementCreatedEvent elementCreatedEvent => elementCreatedEvent.ToDomain(),
            WindowsElementDeletedEvent elementDeletedEvent => elementDeletedEvent.ToDomain(),
            _ => throw new NotImplementedException($"Mapping for {@event.GetType()} is not implemented")
        };
    }

    public static WindowsElementCreatedEvent ToWindows(this ElementCreatedEvent @event) => new(@event.UserId, @event.Element.PageId, @event.Element.ToWindows());
    public static WindowsElementDeletedEvent ToWindows(this ElementDeletedEvent @event) => new(@event.UserId, @event.BookId, @event.PageId, @event.ElementId);

    public static ElementCreatedEvent ToDomain(this WindowsElementCreatedEvent @event) => new(@event.Element.ToDomain()) { UserId = @event.UserId };
    public static ElementDeletedEvent ToDomain(this WindowsElementDeletedEvent @event) => new(@event.BookId, @event.PageId, @event.ElementId) { UserId = @event.UserId };
    #endregion
}
