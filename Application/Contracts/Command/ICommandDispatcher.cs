namespace Application.Contracts.Command;
public interface ICommandDispatcher {
    Task<TResult> PublishAsync<TCommand, TResult>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TResult>;
    Task PublishAsync<TCommand>(TCommand command, CancellationToken ct = default) where TCommand : ICommand;
}
