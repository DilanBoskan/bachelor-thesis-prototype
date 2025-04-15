using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages;
public interface IMessageListener {
    IReadOnlyList<T> ReceiveRecent<T>(DateTime lastUpdate) where T : IMessage;
}
