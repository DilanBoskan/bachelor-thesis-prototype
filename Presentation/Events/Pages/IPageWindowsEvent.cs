using Domain.Aggregates.Pages;
using Presentation.Events;
using Presentation.Models.Elements;
using System;

namespace Presentation.Events.Pages;
public interface IPageWindowsEvent : IWindowsEvent {
    PageId PageId { get; }
}