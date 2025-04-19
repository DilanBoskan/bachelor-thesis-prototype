using Domain.Entities.Books;

namespace Application.Services.Messages;

public interface IMessageManagerFactory {
    IMessageManager Create(BookId bookId);
}
