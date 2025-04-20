using Domain.Entities.Books;

namespace Application.Services.Events;

public interface IEventManagerFactory {
    IEventManager Create(BookId bookId);
}
