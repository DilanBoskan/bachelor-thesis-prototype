using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Command;
public interface ICommandDispatcher {
    Task<TResult> PublishAsync<TCommand, TResult>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TResult>;
    Task PublishAsync<TCommand>(TCommand command, CancellationToken ct = default) where TCommand : ICommand;
}
