using Application.Extensions.Serializer.Books;
using Application.Extensions.Serializer.Elements;
using Application.Extensions.Serializer.Pages;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions.Serializer.Events;
public static class ElementDeletedEventSerializer {
    public static ElementDeletedEvent ToDomain(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.ElementCreated) throw new ArgumentException("Invalid proto type", nameof(proto));

        var userId = UserIdSerializer.ToDomain(proto.UserId);
        var bookId = BookIdSerializer.ToDomain(proto.ElementDeleted.BookId);
        var pageId = PageIdSerializer.ToDomain(proto.ElementDeleted.PageId);
        var elementId = ElementIdSerializer.ToDomain(proto.ElementDeleted.ElementId);

        return new ElementDeletedEvent(bookId, pageId, elementId) {
            UserId = userId
        };
    }

    public static Protos.Events.Event ToProto(ElementDeletedEvent value) {
        var userId = UserIdSerializer.ToProto(value.UserId);
        var bookId = BookIdSerializer.ToProto(value.BookId);
        var pageId = PageIdSerializer.ToProto(value.PageId);
        var elementId = ElementIdSerializer.ToProto(value.ElementId);

        var elementDeleted = new Protos.Events.ElementDeletedEvent() {
            BookId = bookId,
            PageId = pageId,
            ElementId = elementId
        };

        return new Protos.Events.Event() {
            UserId = userId,
            ElementDeleted = elementDeleted
        };
    }
}
