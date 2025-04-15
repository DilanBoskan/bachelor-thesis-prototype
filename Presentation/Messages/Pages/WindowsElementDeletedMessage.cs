using Domain.Entities.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Messages.Pages;
public record WindowsElementDeletedMessage(DateTime TimeGenerated, ElementId ElementId) : IWindowsMessage;
