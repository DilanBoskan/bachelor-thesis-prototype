using Domain.Aggregates;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;

namespace Application.Extensions.Serializer;

public static class UserIdSerializer {
    public static UserId ToDomain(string proto) => UserId.Create(Guid.Parse(proto));
    public static string ToProto(UserId value) => value.Value.ToString();
}
