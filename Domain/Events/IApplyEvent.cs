using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events;
public interface IApplyEvent<TEvent> where TEvent : IEvent {
    void Apply(TEvent @event);
}
