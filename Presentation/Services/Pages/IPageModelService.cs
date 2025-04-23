using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Presentation.Models.Elements.InkStrokes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace Presentation.Services.Pages;
public interface IPageModelService {
    Task<PageModelContent> GetContentAsync(PageId id, CancellationToken ct = default);

    Task<InkStrokeElementModel> CreateInkStrokeElementAsync(BookId bookId, PageId id, DateTime createdAt, InkStrokePoint[] points, CancellationToken ct = default);
}
