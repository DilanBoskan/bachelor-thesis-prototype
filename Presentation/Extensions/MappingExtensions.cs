using Domain.Entities.Books;
using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using Domain.Messages;
using Domain.Messages.Pages;
using Presentation.Messages.Pages;
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

        builder.SetDefaultDrawingAttributes(new InkDrawingAttributes() {
            Color = Windows.UI.Colors.Red,
            IgnorePressure = false,
            FitToCurve = true,
            PenTip = PenTipShape.Circle,
            Size = new Size(10, 10)
        });

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

    #region Messages
    public static IWindowsMessage ToWindows(this IMessage message) {
        return message switch {
            ElementCreatedMessage elementCreatedMessage => new WindowsElementCreatedMessage(elementCreatedMessage.TimeGenerated, elementCreatedMessage.PageId, elementCreatedMessage.Element.ToWindows()),
            ElementDeletedMessage elementDeletedMessage => new WindowsElementDeletedMessage(elementDeletedMessage.TimeGenerated, elementDeletedMessage.PageId, elementDeletedMessage.ElementId),
            _ => throw new NotImplementedException($"Mapping for {message.GetType()} is not implemented")
        };
    }
    public static IMessage ToDomain(this IWindowsMessage message) {
        return message switch {
            WindowsElementCreatedMessage elementCreatedMessage => new ElementCreatedMessage(elementCreatedMessage.TimeGenerated, elementCreatedMessage.PageId, elementCreatedMessage.Element.ToDomain()),
            WindowsElementDeletedMessage elementDeletedMessage => new ElementDeletedMessage(elementDeletedMessage.TimeGenerated, elementDeletedMessage.PageId, elementDeletedMessage.ElementId),
            _ => throw new NotImplementedException($"Mapping for {message.GetType()} is not implemented")
        };
    }
    #endregion
}
