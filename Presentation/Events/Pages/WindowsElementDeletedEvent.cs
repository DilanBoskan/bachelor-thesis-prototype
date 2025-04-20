using Domain.Entities.Elements;
using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events.Pages;
public record WindowsElementDeletedEvent(DateTime TimeGenerated, PageId PageId, ElementId ElementId) : IPageWindowsEvent;
