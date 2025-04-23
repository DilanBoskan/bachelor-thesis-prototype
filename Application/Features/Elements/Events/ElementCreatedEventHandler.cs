using Application.Contracts.Event;
using Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Elements.Events;
public class ElementCreatedEventHandler(ILogger<ElementCreatedEventHandler> logger) : IEventHandler<ElementCreatedEvent> {
    private readonly ILogger<ElementCreatedEventHandler> _logger = logger;
    public Task HandleAsync(ElementCreatedEvent notification, CancellationToken ct = default) {
        _logger.LogInformation("Element created: {ElementId}", notification.Element.Id);

        return Task.CompletedTask;
    }
}
