using Domain.Common;
using Domain.Entities.Pages;

namespace Domain.Entities.Elements;

public sealed record ElementId(Guid Value) : IProtoSerializeable<ElementId, string> {
    public static ElementId New() => new(Guid.NewGuid());
    public static ElementId Create(Guid value) => new(value);


    public static ElementId FromProto(string proto) => ElementId.Create(Guid.Parse(proto));
    public static string ToProto(ElementId value) => value.Value.ToString();
}
