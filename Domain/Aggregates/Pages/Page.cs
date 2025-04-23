using Domain.Aggregates.Books;
using System.Drawing;

namespace Domain.Aggregates.Pages;

public sealed record Page(PageId Id, BookId BookId, SizeF Size);
