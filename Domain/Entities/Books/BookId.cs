using Domain.Entities.Elements;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Books;

public sealed record BookId(Guid Value) : BaseId<BookId>(Value), IBaseId<BookId> {
    public static BookId New() => new(Guid.NewGuid());
    public static BookId Create(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}