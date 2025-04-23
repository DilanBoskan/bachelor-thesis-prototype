using Application.Contracts.Command;
using Application.Contracts.Services;
using Application.Features.Elements.Commands;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Elements;
public class ElementService(ICommandDispatcher commandDispatcher) : IElementService {
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    public async Task<InkStrokeElement> CreateInkStrokeElementAsync(BookId bookId, PageId pageId, DateTime createdAt, IReadOnlyList<InkStrokePoint> points) {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(points);

        var inkStroke = await _commandDispatcher.PublishAsync<CreateInkStrokeElementCommand, InkStrokeElement>(new(bookId, pageId, createdAt, points));

        return inkStroke;
    }

    public async Task DeleteElementAsync(BookId bookId, PageId pageId, ElementId elementId) {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(pageId);
        ArgumentNullException.ThrowIfNull(elementId);

        await _commandDispatcher.PublishAsync<DeleteElementCommand>(new(bookId, pageId, elementId));
    }
}
