using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Events;

public interface IEventManager {
    Task FlushAsync();
    Task<IReadOnlyList<Event>> GetEventsAsync();
}
