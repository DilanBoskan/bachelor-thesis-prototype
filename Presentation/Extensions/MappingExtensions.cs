using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Presentation.Models.Books;
using Presentation.Models.Elements;
using Presentation.Models.Elements.InkStrokes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Input.Inking;

namespace Presentation.Extensions;
public static class MappingExtensions {
    public static BookModel ToWindows(this Book book) => new(book);
    public static PageModel ToWindows(this Page page) => new(page);


    public static ElementModel ToWindows(this Element element) => element switch {
        InkStrokeElement inkStrokeElement => inkStrokeElement.ToWindows(),
        _ => throw new NotImplementedException($"Mapping for {element.GetType()} is not implemented")
    };

    public static InkStrokeElementModel ToWindows(this InkStrokeElement inkStrokeElement) => new(inkStrokeElement);
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
}
