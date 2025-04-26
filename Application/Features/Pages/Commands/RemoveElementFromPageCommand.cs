using Application.Contracts.Command;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Pages;

namespace Application.Features.Pages.Commands;

public record RemoveElementFromPageCommand(PageId PageId, ElementId ElementId) : ICommand;