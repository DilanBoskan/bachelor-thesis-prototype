using Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;
public interface IEventListener {
    Task<IReadOnlyList<Event>> GetEventsAsync(BookId bookId, DateTime from);
}
