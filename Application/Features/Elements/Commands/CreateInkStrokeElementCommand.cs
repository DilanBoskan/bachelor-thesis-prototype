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

namespace Application.Features.Elements.Commands;

public record CreateInkStrokeElementCommand(BookId BookId, PageId PageId, DateTime CreatedAt, IReadOnlyList<InkStrokePoint> Points) : ICommand<InkStrokeElement>;