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
public class RemoveElementFromPageCommandHandler(IPageRepository pageRepository) : ICommandHandler<RemoveElementFromPageCommand> {
    private readonly IPageRepository _pageRepository = pageRepository;

    public async Task HandleAsync(RemoveElementFromPageCommand command, CancellationToken ct = default) {
        var page = await _pageRepository.GetByIdAsync(command.PageId, ct);
        ArgumentNullException.ThrowIfNull(page);

        page.RemoveElement(command.ElementId);

        await _pageRepository.UpdateAsync(page, ct);

        await _pageRepository.SaveChangesAsync(ct);
    }
}
