using Domain.Entities.Elements;
using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages.Pages;

public record ElementCreatedMessage(DateTime TimeGenerated, PageId PageId, Element Element) : IPageMessage;
