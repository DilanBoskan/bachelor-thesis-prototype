using Application.Contracts.Command;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Pages.Commands;
public class CreateElementInPageCommandHandler(IPageRepository pageRepository) : ICommandHandler<CreateInkStrokeElementInPageCommand, InkStrokeElement> {
    private readonly IPageRepository _pageRepository = pageRepository;

    public async Task<InkStrokeElement> HandleAsync(CreateInkStrokeElementInPageCommand command, CancellationToken ct = default) {
        var page = await _pageRepository.GetByIdAsync(command.PageId, ct);
        ArgumentNullException.ThrowIfNull(page);

        var element = page.CreateInkStrokeElement(command.CreatedAt, command.Points);

        await _pageRepository.UpdateAsync(page, ct);

        await _pageRepository.SaveChangesAsync(ct);

        return element;
    }
}
