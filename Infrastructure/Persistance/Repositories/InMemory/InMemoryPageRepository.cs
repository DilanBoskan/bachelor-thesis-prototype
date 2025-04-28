using Application.Contracts.Event;
using Domain.Aggregates.Pages;
using System.Drawing;

namespace Infrastructure.Persistance.Repositories.InMemory;
public class InMemoryPageRepository(IEventDispatcher eventDispatcher) : IPageRepository {
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

    public Task<Page?> GetByIdAsync(PageId id, CancellationToken ct = default) {
        return Task.FromResult(_pages.TryGetValue(id, out var page) ? page : null); 
    }

    public Task UpdateAsync(Page page, CancellationToken ct = default) {
        _transactionPages[page.Id] = page;

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default) {
        var events = _transactionPages.Values
            .SelectMany(e => e.PopDomainEvents())
            .ToList();

        // Save changes (EF Core)
        _pages = [
            .. _pages,
            .. _transactionPages
        ];

        if (events.Count != 0) {
            await _eventDispatcher.PublishAsync(events, CancellationToken.None);
        }

        _transactionPages.Clear();
    }

    private static Dictionary<PageId, Page> _pages = new() {
        { PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000001")), new Page(PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000001")), ReplicationId.New(), new SizeF(1000, 1414), DateTime.UtcNow, DateTime.UtcNow, []) },
        { PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000002")), new Page(PageId.Create(Guid.Parse("00000000-0000-0000-0000-000000000002")), ReplicationId.New(), new SizeF(1000, 1414), DateTime.UtcNow, DateTime.UtcNow, []) },
    };
    private static readonly Dictionary<PageId, Page> _transactionPages = [];
}


public static class DictionaryExtensions {
    public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair) where TKey : notnull => dictionary[keyValuePair.Key] = keyValuePair.Value;
}