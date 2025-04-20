using Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages;
public interface IMessagePublisher {
    void Publish(Message message);
    Task FlushAsync(BookId bookId);
}
