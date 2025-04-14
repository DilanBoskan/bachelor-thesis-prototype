using Domain.Entities.Elements.InkStrokes;
using Presentation.Models.Elements.InkStrokes;
using System.Collections.Generic;
using Windows.UI.Input.Inking;

namespace Presentation.Services.Pages;

public record WindowsPageContent(InkStrokeElementModel[] InkStrokes);
