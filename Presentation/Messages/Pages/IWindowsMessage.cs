using Domain.Entities.Pages;
using Presentation.Models.Elements;
using System;

namespace Presentation.Messages.Pages;
public interface IPageWindowsMessage : IWindowsMessage {
    PageId PageId { get; }
}