using Domain.Aggregates.Pages;

namespace Domain.Aggregates.Books;

public sealed record BookContent(IReadOnlyList<Page> Pages);
