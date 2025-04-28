using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.Pages;
using Presentation.Models;
using Presentation.Models.Pages;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.ViewModels.Components;


public partial class PageViewModel : ObservableObjectWithResources<PageId> {
    [ObservableProperty]
    public partial PageModel? Page { get; private set; }

    protected override async Task CreateResourcesAsync(PageId pageId, CancellationToken ct) {
        var page = new PageModel(pageId);
        await page.ActivateAsync(ct);

        Page = page;
    }

    protected override async Task ReleaseResourcesAsync() {
        if (Page is not null) {
            await Page.DeactivateAsync();
            Page = null;
        }
    }
}
