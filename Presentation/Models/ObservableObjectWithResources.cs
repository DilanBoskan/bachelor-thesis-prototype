using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Models;

public abstract partial class ObservableObjectWithResources<TArgs> : ObservableObject where TArgs : class {
    [ObservableProperty]
    public partial TArgs Arguments { get; private set; }
    [ObservableProperty]
    public partial bool IsActive { get; private set; }
    [ObservableProperty]
    public partial bool IsResourcesCreated { get; private set; }


    public virtual async Task ActivateAsync(TArgs args, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(args);

        // Prepare for reload
        if (IsActive && Arguments != args)
            await DeactivateAsync();

        Arguments = args;
        IsActive = true;
        await SafeCreateResourcesAsync(args, ct);
    }
    public virtual async Task DeactivateAsync() {
        await SafeReleaseResourcesAsync();
        IsActive = false;
        Arguments = null!;
    }

    private readonly SemaphoreSlim _resourcesSemaphore = new(1);
    private CancellationTokenSource? _createResourcesCts;
    private readonly Lock _ctsLock = new();
    protected async Task SafeCreateResourcesAsync(TArgs args, CancellationToken ct = default) {
        ct.ThrowIfCancellationRequested();

        await _resourcesSemaphore.WaitAsync(ct);
        try {
            if (IsResourcesCreated)
                return;

            lock (_ctsLock) {
                _createResourcesCts?.Dispose();
                _createResourcesCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            }

            await CreateResourcesAsync(args, _createResourcesCts.Token);

            IsResourcesCreated = true;
        } catch (OperationCanceledException) {
            await ReleaseResourcesAsync();
            ct.ThrowIfCancellationRequested(); // Throw if the original ct is cancelled
            return;
        } finally {
            _resourcesSemaphore.Release();
        }
    }
    protected async Task SafeReleaseResourcesAsync() {
        lock (_ctsLock) {
            _createResourcesCts?.Cancel();
            _createResourcesCts?.Dispose();
            _createResourcesCts = null;
        }

        await _resourcesSemaphore.WaitAsync();
        try {
            if (!IsResourcesCreated)
                return;
            IsResourcesCreated = false;

            await ReleaseResourcesAsync();
        } finally {
            _resourcesSemaphore.Release();
        }
    }

    protected abstract Task CreateResourcesAsync(TArgs args, CancellationToken ct);
    protected abstract Task ReleaseResourcesAsync();

    private TArgs _args;
}

public abstract partial class ObservableObjectWithResources : ObservableObject {
    [ObservableProperty]
    public partial bool IsActive { get; private set; }

    [ObservableProperty]
    public partial bool IsResourcesCreated { get; private set; }

    public virtual async Task ActivateAsync(CancellationToken ct = default) {
        IsActive = true;
        await SafeCreateResourcesAsync(ct);
    }
    public virtual async Task DeactivateAsync() {
        await SafeReleaseResourcesAsync();
        IsActive = false;
    }

    private readonly SemaphoreSlim _resourcesSemaphore = new(1);
    private CancellationTokenSource? _createResourcesCts;
    private readonly Lock _ctsLock = new();
    protected async Task SafeCreateResourcesAsync(CancellationToken ct = default) {
        ct.ThrowIfCancellationRequested();

        await _resourcesSemaphore.WaitAsync(ct);
        try {
            if (IsResourcesCreated)
                return;

            lock (_ctsLock) {
                _createResourcesCts?.Dispose();
                _createResourcesCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            }

            await CreateResourcesAsync(_createResourcesCts.Token);

            IsResourcesCreated = true;
        } catch (OperationCanceledException) {
            await ReleaseResourcesAsync();
            ct.ThrowIfCancellationRequested(); // Throw if the original ct is cancelled
            return;
        } finally {
            _resourcesSemaphore.Release();
        }
    }
    protected async Task SafeReleaseResourcesAsync() {
        lock (_ctsLock) {
            _createResourcesCts?.Cancel();
            _createResourcesCts?.Dispose();
            _createResourcesCts = null;
        }

        await _resourcesSemaphore.WaitAsync();
        try {
            if (!IsResourcesCreated)
                return;
            IsResourcesCreated = false;

            await ReleaseResourcesAsync();
        } finally {
            _resourcesSemaphore.Release();
        }
    }

    protected abstract Task CreateResourcesAsync(CancellationToken ct);
    protected abstract Task ReleaseResourcesAsync();
}