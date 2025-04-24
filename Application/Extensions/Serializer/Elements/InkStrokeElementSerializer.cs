using Application.Extensions.Serializer.Books;
using Application.Extensions.Serializer.Pages;
using Domain.Aggregates.Elements.InkStrokes;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System.Numerics;

namespace Application.Extensions.Serializer.Elements;

public static class InkStrokeElementSerializer {
    public static InkStrokeElement ToDomain(Protos.Elements.Element proto) {
        if (proto.ElementCase != Protos.Elements.Element.ElementOneofCase.InkStroke) throw new ArgumentException("Invalid proto type", nameof(proto));

        var id = ElementIdSerializer.ToDomain(proto.Id);
        var bookId = BookIdSerializer.ToDomain(proto.BookId);
        var pageId = PageIdSerializer.ToDomain(proto.PageId);
        var createdAt = proto.CreatedAt.ToDateTime();
        var updatedAt = proto.UpdatedAt.ToDateTime();
        var inkPoints = proto.InkStroke.Points
            .Select(p => new InkStrokePoint(new Vector2(p.PositionX, p.PositionY), p.Pressure))
            .ToArray();

        return InkStrokeElement.Load(
            id,
            bookId,
            pageId,
            createdAt,
            updatedAt,
            inkPoints
        );
    }

    public static Protos.Elements.Element ToProto(InkStrokeElement value) {
        var id = ElementIdSerializer.ToProto(value.Id);
        var bookId = BookIdSerializer.ToProto(value.BookId);
        var pageId = PageIdSerializer.ToProto(value.PageId);
        var createdAt = value.CreatedAt.ToTimestamp();
        var updatedAt = value.UpdatedAt.ToTimestamp();
        var inkPoints = value.Points
            .Select(v => new Protos.Elements.InkStrokePoint() {
                PositionX = v.Position.X,
                PositionY = v.Position.Y,
                Pressure = v.Pressure
            });

        var inkStrokeProto = new Protos.Elements.InkStrokeElement();
        inkStrokeProto.Points.AddRange(inkPoints);

        return new Protos.Elements.Element() {
            Id = id,
            BookId = bookId,
            PageId = pageId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            InkStroke = inkStrokeProto,
        };
    }
}