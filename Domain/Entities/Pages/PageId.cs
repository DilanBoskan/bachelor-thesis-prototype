using Domain.Common;

namespace Domain.Entities.Pages;

public sealed record PageId(Guid Value) : IProtoSerializeable<PageId, string> {
    public static PageId New() => new(Guid.NewGuid());
    public static PageId Create(Guid value) => new(value);

    public static PageId FromProto(string proto) => PageId.Create(Guid.Parse(proto));
    public static string ToProto(PageId value) => value.Value.ToString();
}