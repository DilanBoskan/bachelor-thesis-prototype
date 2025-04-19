using Domain.Entities.Books;
using Domain.Messages;
using System.Text.Json.Serialization;

namespace Infrastructure.Extensions;

[JsonSerializable(typeof(Message[]))]
[JsonSerializable(typeof(ElementCreatedMessage))]
[JsonSerializable(typeof(ElementDeletedMessage))]
public partial class AppJsonSerializerContext : JsonSerializerContext {
}
