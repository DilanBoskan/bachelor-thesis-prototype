using Application.Contracts.Event;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event;
public sealed class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher {
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public Task PublishAsync(IEvent @event, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(@event);

        var eventType = @event.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

        var handler = (dynamic)_serviceProvider.GetRequiredService(handlerType);

        return handler.HandleAsync(@event, ct);
    }
}
