using Domain.Aggregates.Books;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.Common;
public abstract record BaseId<TSelf> : IParsable<TSelf> where TSelf : BaseId<TSelf>, new() {
    public Guid Value { get; init; }


    public static TSelf New() => new() { Value = Guid.NewGuid() };
    public static TSelf Create(Guid value) => new() { Value = value };

    public static TSelf Parse(string s, IFormatProvider? provider) => new() { Value = Guid.Parse(s) };
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) {
        if (Guid.TryParse(s, out var guid)) {
            result = new() { Value = guid };
            return true;
        }
        result = default;
        return false;
    }

    public override string ToString() => Value.ToString();
}