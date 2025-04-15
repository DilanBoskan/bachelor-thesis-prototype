using Presentation.Models.Elements;
using System;

namespace Presentation.Messages.Pages;
public interface IWindowsMessage {
    DateTime TimeGenerated { get; }
}