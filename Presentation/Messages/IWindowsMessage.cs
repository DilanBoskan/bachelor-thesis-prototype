using Presentation.Models.Elements;
using System;

namespace Presentation.Messages;
public interface IWindowsMessage {
    DateTime TimeGenerated { get; }
}