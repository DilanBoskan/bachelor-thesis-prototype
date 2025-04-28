namespace Domain.Events;
public interface IApplyEvent {
    void Apply(Event @event);
}
