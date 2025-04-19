using Domain.Entities.Books;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Messages.Local;
public partial class MessageDispatcher : IMessageListener {
    public async Task<IReadOnlyList<Message>> GetEventsAsync(BookId bookId, DateTime from) {
        var events = await _eventsClient.GetEventsAsync(bookId.Value, userId, from);

        return events;
    }
}
