using Presentation.Models.Page;
using System.Collections.Generic;

namespace Presentation.Services.Books;

public record BookModelContent(IReadOnlyList<PageModel> Pages);