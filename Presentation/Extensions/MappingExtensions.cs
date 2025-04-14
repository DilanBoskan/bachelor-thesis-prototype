using Domain.Entities.Books;
using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
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
    public static BookModel ToBookModel(this Book book) => new(book);
    public static Book ToBook(this BookModel book) => new(book.Id);

    public static PageModel ToPageModel(this Page page) => new(page);
    public static Page ToPage(this PageModel page) => new(page.Id, page.Size);


    public static ElementModel ToElementModel(this Element element) => element switch {
        InkStrokeElement inkStrokeElement => new InkStrokeElementModel(inkStrokeElement),
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
}
