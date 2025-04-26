namespace Domain.Events;
public interface IApplyEvent {
    void Apply(IEvent @event);
}
