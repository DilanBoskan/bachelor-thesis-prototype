using Domain.Entities.Elements;
using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages.Pages;
public record ElementDeletedMessage(DateTime TimeGenerated, PageId PageId, ElementId ElementId) : IPageMessage;
