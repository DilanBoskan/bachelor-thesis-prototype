using Domain.Entities.Pages;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services.Pages;
public interface IWindowsPageService {
    Task<WindowsPageContent> GetContentAsync(PageId id, CancellationToken ct = default);
}
