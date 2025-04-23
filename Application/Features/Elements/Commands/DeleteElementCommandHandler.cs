using Application.Contracts.Command;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Elements.Commands;
public class DeleteElementCommandHandler(IElementRepository elementRepository) : ICommandHandler<DeleteElementCommand> {
    private readonly IElementRepository _elementRepository = elementRepository;

    public async Task HandleAsync(DeleteElementCommand command, CancellationToken ct = default) {
        await _elementRepository.DeleteAsync(command.ElementId, ct);

        await _elementRepository.SaveChangesAsync(ct);
    }
}
