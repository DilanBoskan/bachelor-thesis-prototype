using Domain.Aggregates.Pages;

namespace Application.Extensions.Serializer.Pages;

public static class PageIdSerializer {
    public static PageId ToDomain(string proto) => PageId.Create(Guid.Parse(proto));
    public static string ToProto(PageId value) => value.Value.ToString();
}
