using Domain.Entities.Elements;
using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Messages.Pages;
public record WindowsElementDeletedMessage(DateTime TimeGenerated, PageId PageId, ElementId ElementId) : IPageWindowsMessage;
