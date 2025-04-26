namespace Domain.Aggregates.Books;
public interface IBookRepository {
    Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default);
}
