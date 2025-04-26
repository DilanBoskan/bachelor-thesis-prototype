namespace Application.Contracts.Command;

internal interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult> {
    Task<TResult> HandleAsync(TCommand command, CancellationToken ct = default);
}

internal interface ICommandHandler<TCommand> where TCommand : ICommand {
    Task HandleAsync(TCommand command, CancellationToken ct = default);
}