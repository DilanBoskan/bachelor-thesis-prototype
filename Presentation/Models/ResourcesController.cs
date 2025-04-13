using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Models;
public abstract partial class ResourcesController : ObservableObject {
    [ObservableProperty]
    public partial bool IsActive { get; private set; }

    [ObservableProperty]
    public partial bool IsResourcesCreated { get; private set; }


    public async Task ActivateAsync(CancellationToken ct = default) {
        IsActive = true;
        await SafeCreateResourcesAsync(ct);
    }
    public async Task DeactivateAsync() {
        await SafeReleaseResourcesAsync();
        IsActive = false;
    }

    private readonly SemaphoreSlim _resourcesSemaphore = new(1);
    private CancellationTokenSource? _createResourcesCts;
    private readonly Lock _ctsLock = new();
    private async Task SafeCreateResourcesAsync(CancellationToken ct = default) {
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
    private async Task SafeReleaseResourcesAsync() {
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
