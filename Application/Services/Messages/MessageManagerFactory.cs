using Domain.Entities.Books;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Messages;
public class MessageManagerFactory(IMessagePublisher messagePublisher, IMessageListener messageListener) : IMessageManagerFactory {
    public IMessageManager Create(BookId bookId) => new MessageManager(bookId, messagePublisher, messageListener);
}
