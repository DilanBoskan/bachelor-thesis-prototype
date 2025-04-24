using Domain.Aggregates.Books;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Cloud;

public interface ICloudManagerFactory {
    ICloudManager Create(Guid instanceId, BookId bookId);
}
