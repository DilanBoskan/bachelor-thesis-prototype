using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Messages.InMemory;
public partial class InMemoryMessageDispatcher : IMessageListener {
    public IReadOnlyList<T> ReceiveRecent<T>(DateTime lastUpdate) where T : IMessage {
        List<T> events;

        lock (_queueLock) {
            events = _publishedMessages
                .Where(e => e.Key > lastUpdate && e.Value.UserId != _userId && e.Value is T) // Not self-published events
                .Select(e => e.Value.Message)
                .Cast<T>()
                .ToList();
        }

        return events;
    }
}
