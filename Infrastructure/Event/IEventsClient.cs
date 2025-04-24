using Domain.Aggregates.Books;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event;

public interface IEventsClient {
    [Get("/events/{bookId}")]
    Task<byte[]> PullAsync(BookId bookId, [Query] Guid instanceId, [Query] DateTime from);

    [Post("/events/{bookId}")]
    Task PushAsync(BookId bookId, [Query] Guid instanceId, [Body] byte[] data);
}
