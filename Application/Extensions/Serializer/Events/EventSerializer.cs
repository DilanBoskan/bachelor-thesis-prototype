using Application.Protos;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Events;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions.Serializer.Events;

public static class EventSerializer {
    public static IEvent ToDomain(Protos.Events.Event proto) {
        return proto.EventCase switch {
            Protos.Events.Event.EventOneofCase.ElementCreated => ElementCreatedEventSerializer.ToDomain(proto),
            Protos.Events.Event.EventOneofCase.ElementDeleted => ElementDeletedEventSerializer.ToDomain(proto),
            _ => throw new NotImplementedException()
        };
    }

    public static Protos.Events.Event ToProto(IEvent value) {
        return value switch {
            ElementCreatedEvent elementCreated => ElementCreatedEventSerializer.ToProto(elementCreated),
            ElementDeletedEvent elementDeleted => ElementDeletedEventSerializer.ToProto(elementDeleted),
            _ => throw new NotImplementedException()
        };
    }
}
