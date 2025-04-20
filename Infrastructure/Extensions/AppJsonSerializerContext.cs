using Domain.Entities.Books;
using System.Text.Json.Serialization;

namespace Infrastructure.Extensions;

[JsonSerializable(typeof(byte[]))]
[JsonSerializable(typeof(BookId))]
public partial class AppJsonSerializerContext : JsonSerializerContext {
}
