using Application.Contracts.Event;
using Domain.Aggregates.Pages;
using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Repositories.InMemory;
public class InMemoryPageRepository(IEventDispatcher eventDispatcher) : IPageRepository {
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

    public Task<Page?> GetByIdAsync(PageId id, CancellationToken ct = default) {
        return Task.FromResult(_pages.TryGetValue(id, out var page) ? page : null); 
    }

    public Task UpdateAsync(Page page, CancellationToken ct = default) {
        _pages[page.Id] = page;
        _modifiedPages.Add(page);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default) {
        var events = _modifiedPages
            .SelectMany(e => e.PopDomainEvents())
            .ToList();

        // Save changes (EF Core)

        await _eventDispatcher.PublishAsync(events, CancellationToken.None);

        _modifiedPages.Clear();
    }

    private readonly List<Page> _modifiedPages = [];
    private static readonly Dictionary<PageId, Page> _pages = [];
}
