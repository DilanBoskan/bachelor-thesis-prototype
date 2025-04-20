using Domain.Entities.Books;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Events;
public class EventManagerFactory(IEventPublisher messagePublisher, IEventListener messageListener) : IEventManagerFactory {
    public IEventManager Create(BookId bookId) => new EventManager(bookId, messagePublisher, messageListener);
}
