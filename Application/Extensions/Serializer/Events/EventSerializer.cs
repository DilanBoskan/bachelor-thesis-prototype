using Domain.Events;

namespace Application.Extensions.Serializer.Events;

public static class EventSerializer {
    public static IEvent ToDomain(Protos.Events.Event proto) {
        return proto.EventCase switch {
            Protos.Events.Event.EventOneofCase.InkStrokeElementAddedToPage => InkStrokeElementAddedToPageEventSerializer.ToDomain(proto),
            Protos.Events.Event.EventOneofCase.ElementRemovedFromPage => ElementRemovedFromPageEventSerializer.ToDomain(proto),
            _ => throw new NotImplementedException()
        };
    }

    public static Protos.Events.Event ToProto(IEvent value) {
        return value switch {
            InkStrokeElementAddedToPageEvent @event => InkStrokeElementAddedToPageEventSerializer.ToProto(@event),
            ElementRemovedFromPageEvent @event => ElementRemovedFromPageEventSerializer.ToProto(@event),
            _ => throw new NotImplementedException()
        };
    }
}
