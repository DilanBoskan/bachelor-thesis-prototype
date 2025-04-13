using Domain.Entities.Elements;
using Domain.Entities.Elements.InkStrokes;
using Domain.Entities.Pages;
using Domain.Helpers;
using System.Drawing;
using System.Numerics;

namespace Application.Services.Pages;
public sealed class DummyPageService : IPageService {
    public async Task<Page> GetAsync(PageId id, CancellationToken ct = default) {
        await DelayHelper.Short(ct);

        return new Page(id, new SizeF(1000, 1414));
    }


    public async Task<PageContent> GetContentAsync(PageId id, CancellationToken ct = default) {
        if (!_pageContent.TryGetValue(id, out var pageContent)) {
            // InkStrokes
            await DelayHelper.Medium(ct); // Simulate loading time
            List<InkStrokeElement> inkStrokes = [CreateRandomInkStroke(), CreateRandomInkStroke()];

            pageContent = _pageContent[id] = new PageContent(inkStrokes);
        }

        return pageContent;
    }

    private static InkStrokeElement CreateRandomInkStroke() {
        var random = new Random();
        var maxPoints = random.Next(100, 200);

        List<InkStrokePoint> points = new(maxPoints);

        var initialPoint = new Vector2(random.Next(50, 800), random.Next(50, 800));
        points.Add(new InkStrokePoint(initialPoint));
        for (var i = 1; i < maxPoints - 1; i++) {
            var prevPoint = points[i - 1];
            var translate = new Vector2(random.Next(-5, 10), random.Next(-5, 10));

            var newPoint = new InkStrokePoint(prevPoint.Position + translate);
            points.Add(newPoint);
        }

        return new InkStrokeElement(ElementId.New(), DateTime.Now, points);
    }

    private readonly Dictionary<PageId, PageContent> _pageContent = [];
}
