using Domain.Entities.Elements;
using Domain.Entities.Pages;
using Presentation.Models.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events.Pages;

public record WindowsElementCreatedEvent(DateTime TimeGenerated, PageId PageId, ElementModel Element) : IPageWindowsEvent;
