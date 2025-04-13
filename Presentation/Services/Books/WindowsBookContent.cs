using Presentation.Models.Page;
using System.Collections.Generic;

namespace Presentation.Services.Books;

public record WindowsBookContent(IReadOnlyList<PageModel> Pages);