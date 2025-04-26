using Domain.Aggregates.Elements;
using Presentation.Models.Elements;
using Presentation.Models.Elements.InkStrokes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Models.Pages;

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
