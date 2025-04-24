using Domain.Aggregates;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events.Pages;
public record WindowsElementDeletedEvent(UserId UserId, BookId BookId, PageId PageId, ElementId ElementId) : IPageWindowsEvent;
