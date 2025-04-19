using Domain.Messages;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messages.Local;

public interface IEventsClient {
    [Get("/events/{bookId}")]
    Task<Message[]> GetEventsAsync(Guid bookId, [Query] Guid userId, [Query] DateTime from);

    [Post("/events/{bookId}")]
    Task PostMessagesAsync(Guid bookId, [Query] Guid userId, [Body] Message[] messages);
}
