using Domain.Aggregates.Elements.InkStrokes;
using Presentation.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Elements.InkStrokes;
public sealed partial class InkStrokeElementModel(InkStrokeElement element) : ElementModel(element.Id, element.CreatedAt, element.UpdatedAt) {
    public InkStroke InkStroke { get; private set; } = element.Points.ToInkStroke();

    /// <summary>
    /// Refreshes the ink stroke element by cloning the current ink stroke so it can be used again later.
    /// </summary>
    public void Refresh() {
        InkStroke = InkStroke.Clone();
    }

    protected override Task CreateResourcesAsync(CancellationToken ct) {
        return Task.CompletedTask;
    }

    protected override Task ReleaseResourcesAsync() {
        return Task.CompletedTask;
    }
}
