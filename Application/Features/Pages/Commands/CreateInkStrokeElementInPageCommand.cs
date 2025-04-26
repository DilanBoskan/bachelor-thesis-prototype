using Application.Contracts.Command;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;

namespace Application.Features.Pages.Commands;

public record CreateInkStrokeElementInPageCommand(PageId PageId, DateTime CreatedAt, IReadOnlyList<InkStrokePoint> Points) : ICommand<InkStrokeElement>;