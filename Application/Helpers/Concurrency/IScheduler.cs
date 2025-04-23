using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Helpers.Concurrency; 

public interface IScheduler : IDisposable {
    public bool IsOperationRunning { get; }

    Task EnqueueAsync(Action action, CancellationToken ct = default);
    Task<T> EnqueueAsync<T>(Func<T> action, CancellationToken ct = default);

    Task EnqueueAsync(Func<Task> asyncAction, CancellationToken ct = default);
    Task<T> EnqueueAsync<T>(Func<Task<T>> asyncAction, CancellationToken ct = default);
}
