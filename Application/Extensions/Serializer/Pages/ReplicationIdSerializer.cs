using Domain.Aggregates.Pages;

namespace Application.Extensions.Serializer.Pages;

public static class ReplicationIdSerializer {
    public static ReplicationId ToDomain(ulong proto) => new(proto);
    public static ulong ToProto(ReplicationId value) => value.Value;
}
