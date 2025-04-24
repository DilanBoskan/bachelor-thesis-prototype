using Domain.Aggregates;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using Presentation.Models.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events.Pages;

public record WindowsElementCreatedEvent(UserId UserId, PageId PageId, ElementModel Element) : IPageWindowsEvent;
