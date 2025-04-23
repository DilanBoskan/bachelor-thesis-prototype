using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Helpers.Concurrency; 
public sealed class SingleThreadTaskScheduler : TaskScheduler, IDisposable {
    public Thread Thread => _thread;

    public SingleThreadTaskScheduler() {
        _thread = new Thread(() => {
            foreach (var task in _queue.GetConsumingEnumerable()) {
                TryExecuteTask(task);
            }
        }) {
            IsBackground = true
        };
        _thread.Start();
    }

    protected override void QueueTask(Task task) => _queue.Add(task);
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => Thread.CurrentThread == _thread && TryExecuteTask(task);
    protected override IEnumerable<Task> GetScheduledTasks() => _queue;

    #region IDisposable
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing) {
        if (_disposed)
            return;

        if (disposing) {
            _queue.CompleteAdding();
            _thread.Join();
            _queue.Dispose();
        }

        _disposed = true;
    }
    private bool _disposed = false;
    #endregion

    private readonly Thread _thread;
    private readonly BlockingCollection<Task> _queue = new();
}
