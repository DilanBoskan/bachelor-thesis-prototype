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
public static class ElementCreatedEventSerializer {
    public static ElementCreatedEvent ToDomain(Protos.Events.Event proto) {
        if (proto.EventCase != Protos.Events.Event.EventOneofCase.ElementCreated) throw new ArgumentException("Invalid proto type", nameof(proto));

        var userId = UserIdSerializer.ToDomain(proto.UserId);
        var element = proto.ElementCreated.Element.ToDomain();

        return new ElementCreatedEvent(element) {
            UserId = userId
        };
    }

    public static Protos.Events.Event ToProto(ElementCreatedEvent value) {
        var userId = UserIdSerializer.ToProto(value.UserId);

        var elementCreated = new Protos.Events.ElementCreatedEvent() {
            Element = value.Element.ToProto()
        };

        return new Protos.Events.Event() {
            UserId = userId,
            ElementCreated = elementCreated
        };
    }
}
