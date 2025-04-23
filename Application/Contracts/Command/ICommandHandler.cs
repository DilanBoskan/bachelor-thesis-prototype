using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Command;

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult> {
    Task<TResult> HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<TCommand> where TCommand : ICommand {
    Task HandleAsync(TCommand command, CancellationToken ct = default);
}