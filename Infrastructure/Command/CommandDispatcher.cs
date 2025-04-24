using Application.Contracts.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Command;


internal sealed class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher {

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public Task<TResult> PublishAsync<TCommand, TResult>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TResult> {
        ArgumentNullException.ThrowIfNull(command);

        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();

        return handler.HandleAsync(command, ct);
    }

    public Task PublishAsync<TCommand>(TCommand command, CancellationToken ct = default) where TCommand : ICommand {
        ArgumentNullException.ThrowIfNull(command);

        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        return handler.HandleAsync(command, ct);
    }
}
