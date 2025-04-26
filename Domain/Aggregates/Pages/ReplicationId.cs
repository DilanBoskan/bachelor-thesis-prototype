using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Domain.Aggregates.Pages;

public sealed record ReplicationId(ulong Value) : IEntity, IParsable<ReplicationId>, IComparisonOperators<ReplicationId, ReplicationId, bool> {
    public static ReplicationId Empty { get; } = new(0);
    public static ReplicationId New() => new(1);

    public ReplicationId Next() => new(Value + 1);
    public override string ToString() => Value.ToString();


    public static ReplicationId Parse(string s, IFormatProvider? provider) => new(ulong.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ReplicationId result) {
        if (ulong.TryParse(s, provider, out var version)) {
            result = new ReplicationId(version);
            return true;
        }
        result = default;
        return false;
    }
    public static bool operator <(ReplicationId left, ReplicationId right) => left.Value < right.Value;
    public static bool operator >(ReplicationId left, ReplicationId right) => left.Value > right.Value;
    public static bool operator <=(ReplicationId left, ReplicationId right) => left.Value < right.Value || left == right;
    public static bool operator >=(ReplicationId left, ReplicationId right) => left.Value > right.Value || left == right;
}
