using Domain.Entities.Elements;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Domain.Entities.Books;

[JsonConverter(typeof(JsonStringEnumConverter))]
public sealed record BookId(Guid Value) {
    public static BookId New() => new(Guid.NewGuid());
    public static BookId Create(Guid value) => new(value);
}