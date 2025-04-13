using System.Collections.Generic;
using Windows.UI.Input.Inking;

namespace Presentation.Services.Pages;

public record WindowsPageContent(List<InkStroke> InkStrokes);
