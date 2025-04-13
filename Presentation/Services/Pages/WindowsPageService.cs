using Application.Services.Pages;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace Presentation.Services.Pages;
public class WindowsPageService(IPageService pageService) : IWindowsPageService {
    private readonly IPageService _pageService = pageService;
    public async Task<WindowsPageContent> GetContentAsync(PageId id, CancellationToken ct = default) {
        var page = await _pageService.GetContentAsync(id, ct);

        var inkStrokes = page.Elements
            .OfType<InkStrokeElement>()
            .Select(MapInkStroke)
            .ToList();

        return new WindowsPageContent(inkStrokes);
    }

    private static InkStroke MapInkStroke(InkStrokeElement inkStrokeElement) {
        var builder = new InkStrokeBuilder();
        builder.SetDefaultDrawingAttributes(new InkDrawingAttributes() {
            Color = Windows.UI.Colors.Red,
            IgnorePressure = false,
            FitToCurve = true,
            PenTip = PenTipShape.Circle,
            Size = new Size(10, 10)
        });

        var points = inkStrokeElement.Points
            .Select(p => new InkPoint(p.Position.ToPoint(), p.Pressure))
            .ToList();
        var stroke = builder.CreateStrokeFromInkPoints(points, Matrix3x2.Identity, inkStrokeElement.CreationDate, TimeSpan.Zero);

        return stroke;
    }
}
