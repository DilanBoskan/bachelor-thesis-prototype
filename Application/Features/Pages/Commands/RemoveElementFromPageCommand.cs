using Application.Contracts.Command;
using Application.Protos;
using Domain.Aggregates.Books;
using Domain.Aggregates.Elements;
using Domain.Aggregates.Elements.InkStrokes;
using Domain.Aggregates.Pages;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Pages.Commands;

public record RemoveElementFromPageCommand(PageId PageId, ElementId ElementId) : ICommand;