namespace Application.Helpers.Concurrency; 
public sealed class DatabaseThreadScheduler(SingleThreadTaskScheduler scheduler) : IScheduler {
    private readonly SingleThreadTaskScheduler _scheduler = scheduler;
    private readonly TaskFactory _factory = new(scheduler);

    public SingleThreadTaskScheduler TaskScheduler => _scheduler;
    public bool IsOperationRunning => _operationsRunning > 0;

    public async Task EnqueueAsync(Action action, CancellationToken ct = default) {
        _operationsRunning++;
        try {
            await _factory.StartNew(action, ct).ConfigureAwait(false);
        } finally {
            _operationsRunning--;
        }
    }
    public async Task<T> EnqueueAsync<T>(Func<T> action, CancellationToken ct = default) {
        _operationsRunning++;
        try {
            return await _factory.StartNew(action, ct).ConfigureAwait(false);
        } finally {
            _operationsRunning--;
        }
    }
    public async Task EnqueueAsync(Func<Task> asyncAction, CancellationToken ct = default) {
        _operationsRunning++;
        try {
            await _factory.StartNew(asyncAction, ct).Unwrap().ConfigureAwait(false);
        } catch (Exception) when (ct.IsCancellationRequested) {
            ct.ThrowIfCancellationRequested();
            throw;
        } finally {
            _operationsRunning--;
        }
    }
    public async Task<T> EnqueueAsync<T>(Func<Task<T>> asyncAction, CancellationToken ct = default) {
        _operationsRunning++;
        try {
            return await _factory.StartNew(asyncAction, ct).Unwrap().ConfigureAwait(false);
        } catch (Exception) when (ct.IsCancellationRequested) {
            ct.ThrowIfCancellationRequested();
            throw;
        } finally {
            _operationsRunning--;
        }
    }

#region IDisposable
    private void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
                _scheduler.Dispose();
            }

            _disposed = true;
        }
    }
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private bool _disposed = false;
#endregion

    private int _operationsRunning = 0;
}
