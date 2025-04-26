using Application.Extensions.Serializer.Elements;
using Domain.Aggregates.Pages;
using Google.Protobuf.WellKnownTypes;
using System.Drawing;

namespace Application.Extensions.Serializer.Pages;

public static class PageSerializer {
    public static Page ToDomain(Protos.Pages.Page proto) {
        var id = PageIdSerializer.ToDomain(proto.Id);
        var replicationId = ReplicationIdSerializer.ToDomain(proto.ReplicationId);
        var width = proto.Width;
        var height = proto.Height;
        var createdAt = proto.CreatedAt.ToDateTime();
        var updatedAt = proto.UpdatedAt.ToDateTime();
        var elements = proto.Elements.Select(ElementSerializer.ToDomain);

        return new Page(id, replicationId, new SizeF(width, height), createdAt, updatedAt, elements);
    }
    public static Protos.Pages.Page ToProto(Page value) {
        var id = PageIdSerializer.ToProto(value.Id);
        var replicationId = ReplicationIdSerializer.ToProto(value.ReplicationId);
        var width = value.Size.Width;
        var height = value.Size.Height;
        var createdAt = value.CreatedAt.ToTimestamp();
        var updatedAt = value.UpdatedAt.ToTimestamp();
        var elements = value.Elements.Select(ElementSerializer.ToProto);

        return new Protos.Pages.Page() {
            Id = id,
            ReplicationId = replicationId,
            Width = width,
            Height = height,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            Elements = { elements }
        };
    }
}
