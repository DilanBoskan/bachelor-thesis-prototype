using Domain.Entities.Pages;

namespace Domain.Entities.Books;

public sealed record BookContent(IReadOnlyList<Page> Pages);
