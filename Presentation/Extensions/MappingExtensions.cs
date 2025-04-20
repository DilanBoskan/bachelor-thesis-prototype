using Domain.Entities.Books;
using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
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
    public static Book ToDomain(this BookModel book) => new(book.Id);

    public static PageModel ToWindows(this Page page) => new(page);
    public static Page ToDomain(this PageModel page) => new(page.Id, page.Size);


    public static ElementModel ToWindows(this Element element) => element switch {
        InkStrokeElement inkStrokeElement => new InkStrokeElementModel(inkStrokeElement),
        _ => throw new NotImplementedException($"Mapping for {element.GetType()} is not implemented")
    };
    public static Element ToDomain(this ElementModel element) => element switch {
        InkStrokeElementModel inkStrokeElementModel => new InkStrokeElement(inkStrokeElementModel.Id, inkStrokeElementModel.CreationDate, inkStrokeElementModel.InkStroke.ToInkStrokePoints()),
        _ => throw new NotImplementedException($"Mapping for {element.GetType()} is not implemented")
    };
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
    public static IWindowsEvent ToWindows(this Event @event) {
        return @event switch {
            ElementCreatedEvent elementCreatedEvent => new WindowsElementCreatedEvent(elementCreatedEvent.TimeGenerated, elementCreatedEvent.PageId, elementCreatedEvent.Element.ToWindows()),
            ElementDeletedEvent elementDeletedEvent => new WindowsElementDeletedEvent(elementDeletedEvent.TimeGenerated, elementDeletedEvent.PageId, elementDeletedEvent.ElementId),
            _ => throw new NotImplementedException($"Mapping for {@event.GetType()} is not implemented")
        };
    }
    public static Event ToDomain(this IWindowsEvent @event) {
        return @event switch {
            WindowsElementCreatedEvent elementCreatedEvent => new ElementCreatedEvent(elementCreatedEvent.TimeGenerated, elementCreatedEvent.PageId, elementCreatedEvent.Element.ToDomain()),
            WindowsElementDeletedEvent elementDeletedEvent => new ElementDeletedEvent(elementDeletedEvent.TimeGenerated, elementDeletedEvent.PageId, elementDeletedEvent.ElementId),
            _ => throw new NotImplementedException($"Mapping for {@event.GetType()} is not implemented")
        };
    }
    #endregion
}
