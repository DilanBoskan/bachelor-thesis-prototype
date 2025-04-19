using Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages;
public interface IMessageListener {
    Task<IReadOnlyList<Message>> GetEventsAsync(BookId bookId, DateTime from);
}
