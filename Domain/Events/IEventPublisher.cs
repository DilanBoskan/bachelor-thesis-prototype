using Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;
public interface IEventPublisher {
    void Publish(Event @event);
    Task FlushAsync(BookId bookId);
}
