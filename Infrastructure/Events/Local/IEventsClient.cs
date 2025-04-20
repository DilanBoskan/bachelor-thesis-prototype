using Domain.Entities.Books;
using Domain.Messages;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events.Local;

public interface IEventsClient {
    [Get("/events/{bookId}")]
    Task<byte[]> GetEventsAsync(BookId bookId, [Query] Guid userId, [Query] DateTime from);

    [Post("/events/{bookId}")]
    Task PostEventsAsync(BookId bookId, [Query] Guid userId, [Body] byte[] data);
}
