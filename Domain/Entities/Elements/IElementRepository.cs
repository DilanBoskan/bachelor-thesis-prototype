using Domain.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Elements;

public interface IElementRepository {
    #region Page Relation
    IReadOnlyList<Element>? GetFromPage(PageId pageId);
    void CreateInPage(PageId pageId, Element id);
    void DeleteInPage(PageId pageId, ElementId id);
    #endregion
}
