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
public partial class InMemoryMessageDispatcher(Guid userId) : IMessagePublisher {
    private readonly Lock _queueLock = new();

    public void Publish(IMessage message) {
        lock (_queueLock) {
            _preparedMessages.Add(DateTime.Now, (_userId, message));
        }
    }

    public void Flush() {
        lock (_queueLock) {
            foreach (var message in _preparedMessages) {
                _publishedMessages.Add(message.Key, message.Value);
            }
            _preparedMessages.Clear();
        }
    }

    private static readonly SortedList<DateTime, (Guid UserId, IMessage Message)> _publishedMessages = [];

    private readonly SortedList<DateTime, (Guid UserId, IMessage Message)> _preparedMessages = [];
    private readonly Guid _userId = userId;
}
