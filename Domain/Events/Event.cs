using Domain.Common;
using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;

public abstract record Event(DateTime TimeGenerated) : IProtoSerializeable<Event, Protos.Events.Event> {
    public static Protos.Events.Event ToProto(Event value) {
        return value switch {
            ElementCreatedEvent elementCreated => ElementCreatedEvent.ToProto(elementCreated),
            ElementDeletedEvent elementDeleted => ElementDeletedEvent.ToProto(elementDeleted),
            _ => throw new NotImplementedException()
        };
    }
    public static Event FromProto(Protos.Events.Event proto) {
        return proto.EventCase switch {
            Protos.Events.Event.EventOneofCase.ElementCreated => ElementCreatedEvent.FromProto(proto),
            Protos.Events.Event.EventOneofCase.ElementDeleted => ElementDeletedEvent.FromProto(proto),
            _ => throw new NotImplementedException()
        };
    }
}
