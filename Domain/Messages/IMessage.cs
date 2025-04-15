using System;

namespace Domain.Messages;
public interface IMessage {
    DateTime TimeGenerated { get; }
}