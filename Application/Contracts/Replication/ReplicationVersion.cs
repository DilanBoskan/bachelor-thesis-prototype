using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Replication;

public sealed record ReplicationVersion(int Value) : IParsable<ReplicationVersion>, IComparisonOperators<ReplicationVersion, ReplicationVersion, bool> {
    public static ReplicationVersion Empty { get; } = new(-1);
    public static ReplicationVersion New() => new(0);

    public ReplicationVersion Next() => new(Value + 1);
    public override string ToString() => Value.ToString();


    public static ReplicationVersion Parse(string s, IFormatProvider? provider) => new(int.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ReplicationVersion result) {
        if (int.TryParse(s, provider, out var version)) {
            result = new ReplicationVersion(version);
            return true;
        }
        result = default;
        return false;
    }
    public static bool operator <(ReplicationVersion left, ReplicationVersion right) => left.Value < right.Value;
    public static bool operator >(ReplicationVersion left, ReplicationVersion right) => left.Value > right.Value;
    public static bool operator <=(ReplicationVersion left, ReplicationVersion right) => left.Value < right.Value || left == right;
    public static bool operator >=(ReplicationVersion left, ReplicationVersion right) => left.Value > right.Value || left == right;
}
