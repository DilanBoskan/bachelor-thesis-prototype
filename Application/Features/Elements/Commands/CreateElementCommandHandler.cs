using Application.Contracts.Command;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Elements.Commands;
public class CreateElementCommandHandler(IElementRepository elementRepository) : ICommandHandler<CreateInkStrokeElementCommand, InkStrokeElement> {
    private readonly IElementRepository _elementRepository = elementRepository;

    public async Task<InkStrokeElement> HandleAsync(CreateInkStrokeElementCommand command, CancellationToken ct = default) {
        var elementId = ElementId.New();

        var element = InkStrokeElement.Create(elementId, command.BookId, command.PageId, command.CreatedAt, command.Points);

        await _elementRepository.CreateAsync(element, ct);

        await _elementRepository.SaveChangesAsync(ct);

        return element;
    }
}
