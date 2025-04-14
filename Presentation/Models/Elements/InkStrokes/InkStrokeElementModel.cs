using Domain.Entities.Elements.InkStrokes;
using Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Elements.InkStrokes;
public sealed partial class InkStrokeElementModel(InkStrokeElement element) : ElementModel(element.Id, element.CreationDate) {
    public InkStroke InkStroke { get; } = element.Points.ToInkStroke();

    protected override Task CreateResourcesAsync(CancellationToken ct) {
        return Task.CompletedTask;
    }

    protected override Task ReleaseResourcesAsync() {
        return Task.CompletedTask;
    }
}
