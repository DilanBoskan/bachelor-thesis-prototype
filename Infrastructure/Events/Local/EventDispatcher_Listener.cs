using Domain.Entities.Books;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Messages.Local;
public partial class EventDispatcher : IEventListener {
    public async Task<IReadOnlyList<Event>> GetEventsAsync(BookId bookId, DateTime from) {
        var eventGroupBytes = await _eventsClient.GetEventsAsync(bookId, userId, from);
        var eventGroup = Domain.Protos.Events.EventGroup.Parser.ParseFrom(eventGroupBytes);

        return eventGroup.Events.Select(Event.FromProto).ToList();  
    }
}
