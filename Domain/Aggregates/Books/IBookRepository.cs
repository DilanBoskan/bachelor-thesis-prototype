using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.Books;
public interface IBookRepository {
    Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default);
}
