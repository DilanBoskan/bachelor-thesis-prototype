using Presentation.Models.Elements;
using System;

namespace Presentation.Events;
public interface IWindowsEvent {
    DateTime TimeGenerated { get; }
}