using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Event;

/// <summary>
/// Handler for internal events
/// </summary>
public interface IEventHandler {
    Task HandleAsync(IReadOnlyList<IEvent> events);
}
