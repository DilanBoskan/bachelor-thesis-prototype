using Domain.Entities.Books;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Messages;
public class MessageManager(BookId bookId, IMessagePublisher messagePublisher, IMessageListener messageListener) : IMessageManager {
    private readonly IMessagePublisher _messagePublisher = messagePublisher;
    private readonly IMessageListener _messageListener = messageListener;

    public Task FlushAsync() => _messagePublisher.FlushAsync(bookId);
    public Task<IReadOnlyList<Message>> GetEventsAsync() {
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
