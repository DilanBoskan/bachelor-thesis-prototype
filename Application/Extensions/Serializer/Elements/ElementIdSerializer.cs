using Domain.Aggregates.Elements;

namespace Application.Extensions.Serializer.Elements;

public static class ElementIdSerializer {
    public static ElementId ToDomain(string proto) => ElementId.Create(Guid.Parse(proto));
    public static string ToProto(ElementId value) => value.Value.ToString();
}
