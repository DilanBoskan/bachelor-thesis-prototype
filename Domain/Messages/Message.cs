using Domain.Common;
using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages;

public abstract record Message(DateTime TimeGenerated) : IProtoSerializeable<Message, Protos.Messages.Message> {
    public static Protos.Messages.Message ToProto(Message value) {
        return value switch {
            ElementCreatedMessage elementCreated => ElementCreatedMessage.ToProto(elementCreated),
            ElementDeletedMessage elementDeleted => ElementDeletedMessage.ToProto(elementDeleted),
            _ => throw new NotImplementedException()
        };
    }
    public static Message FromProto(Protos.Messages.Message proto) {
        return proto.MessageCase switch {
            Protos.Messages.Message.MessageOneofCase.ElementCreated => ElementCreatedMessage.FromProto(proto),
            Protos.Messages.Message.MessageOneofCase.ElementDeleted => ElementDeletedMessage.FromProto(proto),
            _ => throw new NotImplementedException()
        };
    }
}
