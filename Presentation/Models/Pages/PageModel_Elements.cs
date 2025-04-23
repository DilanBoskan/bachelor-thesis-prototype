using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Domain.Aggregates.Elements;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Extensions;
using Presentation.Models.Elements;
using Presentation.Models.Elements.InkStrokes;
using Presentation.Services.Pages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Input.Inking;

namespace Presentation.Models.Page;

public sealed partial class PageModel {
    private void AddElements(IEnumerable<ElementModel> elements) {
        ArgumentNullException.ThrowIfNull(elements);
        foreach (var element in elements) {
            AddElement(element);
        }
    }
    private void AddElement(ElementModel element) {
        ArgumentNullException.ThrowIfNull(element);

        switch (element) {
            case InkStrokeElementModel inkStrokeModel:
                StrokeContainer.AddStroke(inkStrokeModel.InkStroke);
                break;
            default:
                throw new NotImplementedException();
        }

        _elements.Add(element);
    }
    private void RemoveElement(ElementId id) {
        if (_elements.FirstOrDefault(e => e.Id == id) is not ElementModel element)
            return;

        RemoveElement(element);
    }
    private void RemoveElement(ElementModel element) {
        ArgumentNullException.ThrowIfNull(element);
        switch (element) {
            case InkStrokeElementModel inkStrokeModel:
                inkStrokeModel.InkStroke.Selected = true;
                StrokeContainer.DeleteSelected();
                inkStrokeModel.Refresh();
                break;
            default:
                throw new NotImplementedException();
        }

        _elements.Remove(element);
    }
    private void RemoveAllElements() {
        foreach (var element in _elements.ToArray()) {
            RemoveElement(element);
        }
    }


    private readonly HashSet<ElementModel> _elements = [];
}
