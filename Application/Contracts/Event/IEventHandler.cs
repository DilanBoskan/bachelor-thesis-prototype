using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Event;
public interface IEventHandler<TEvent> where TEvent : IEvent {
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}
