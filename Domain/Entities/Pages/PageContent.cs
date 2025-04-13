using Domain.Entities.Elements;

namespace Domain.Entities.Pages;

public sealed record PageContent(IReadOnlyList<Element> Elements);
