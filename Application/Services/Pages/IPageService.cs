using Domain.Entities.Elements;
using Domain.Entities.Pages;

namespace Application.Services.Pages;

public interface IPageService {
    Page Get(PageId id);
    PageContent GetContent(PageId id);

    #region Elements
    void CreateElement<T>(PageId id, T element) where T : Element;
    void DeleteElement(PageId id, ElementId elementId);
    #endregion
}
