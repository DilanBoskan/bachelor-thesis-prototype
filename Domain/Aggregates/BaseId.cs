using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates;
public abstract record BaseId<TSelf>(Guid Value) : IParsable<TSelf> where TSelf : BaseId<TSelf>, IBaseId<TSelf> {
    public static TSelf Parse(string s, IFormatProvider? provider) => TSelf.Create(Guid.Parse(s));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) {
        if (Guid.TryParse(s, out var guid)) {
            result = TSelf.Create(guid);
            return true;
        }
        result = default;
        return false;
    }
}

public interface IBaseId<TSelf> where TSelf : IBaseId<TSelf> {
    static abstract TSelf New();
    static abstract TSelf Create(Guid value);
}