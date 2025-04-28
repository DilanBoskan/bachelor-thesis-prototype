using Domain.Aggregates.Common;

namespace Domain.Aggregates.Pages;

public sealed record PageId : BaseId<PageId> {
    public override string ToString() => base.ToString();
}