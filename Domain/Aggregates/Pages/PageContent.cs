using Domain.Aggregates.Elements;

namespace Domain.Aggregates.Pages;

public sealed record PageContent(IReadOnlyList<Element> Elements);
