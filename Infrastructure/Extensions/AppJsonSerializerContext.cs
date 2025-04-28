using Application.Contracts.Event;
using Domain.Aggregates.Books;
using Domain.Aggregates.Pages;
using System.Text.Json.Serialization;

namespace Infrastructure.Extensions;

[JsonSerializable(typeof(byte[]))]
[JsonSerializable(typeof(PageId))]
[JsonSerializable(typeof(ReplicationId))]
[JsonSerializable(typeof(PullData))]
public partial class AppJsonSerializerContext : JsonSerializerContext {
}
