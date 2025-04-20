using Domain.Entities.Books;
using Domain.Messages;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Messages.Local;
public partial class MessageDispatcher(Guid userId, IEventsClient eventsClient) : IMessagePublisher {
    private readonly IEventsClient _eventsClient = eventsClient;

    private readonly Lock _queueLock = new();
    public void Publish(Message message) {
        lock (_queueLock) {
            _preparedMessages.Add(message);
        }
    }

    public async Task FlushAsync(BookId bookId) {
        Message[] messages;
        lock (_queueLock) {
            if (_preparedMessages.Count == 0)
                return;

            messages = _preparedMessages.ToArray();
            _preparedMessages.Clear();
        }

        var messageGroup = new Domain.Protos.Messages.MessageGroup();
        messageGroup.Messages.AddRange(messages.Select(Message.ToProto));

        await _eventsClient.PostMessagesAsync(bookId, userId, messageGroup.ToByteArray());
    }

    private readonly List<Message> _preparedMessages = [];
}
