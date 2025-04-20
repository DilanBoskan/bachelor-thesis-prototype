using Domain.Entities.Books;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Events;
public class EventManager(BookId bookId, IEventPublisher messagePublisher, IEventListener messageListener) : IEventManager {
    private readonly IEventPublisher _messagePublisher = messagePublisher;
    private readonly IEventListener _messageListener = messageListener;

    public Task FlushAsync() => _messagePublisher.FlushAsync(bookId);
    public Task<IReadOnlyList<Event>> GetEventsAsync() {
        var lastUpdate = _lastUpdated;
        _lastUpdated = DateTime.Now;

        try {
            return _messageListener.GetEventsAsync(bookId, lastUpdate);
        } catch {
            _lastUpdated = lastUpdate;
            throw;
        }
    }


    private DateTime _lastUpdated = DateTime.MinValue;
}
