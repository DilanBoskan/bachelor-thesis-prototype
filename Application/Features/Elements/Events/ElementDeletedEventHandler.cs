using Application.Contracts.Event;
using Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Elements.Events;
public class ElementDeletedEventHandler(ILogger<ElementDeletedEventHandler> logger) : IEventHandler<ElementDeletedEvent> {
    private readonly ILogger<ElementDeletedEventHandler> _logger = logger;
    public Task HandleAsync(ElementDeletedEvent notification, CancellationToken ct = default) {
        _logger.LogInformation("Element deleted: {ElementId}", notification.ElementId);

        return Task.CompletedTask;
    }
}
