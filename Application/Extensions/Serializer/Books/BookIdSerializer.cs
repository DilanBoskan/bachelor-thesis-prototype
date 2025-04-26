using Domain.Aggregates.Books;

namespace Application.Extensions.Serializer.Books;

public static class BookIdSerializer {
    public static BookId ToDomain(string proto) => BookId.Create(Guid.Parse(proto));
    public static string ToProto(BookId value) => value.Value.ToString();
}
