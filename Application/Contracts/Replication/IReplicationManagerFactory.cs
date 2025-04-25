using Domain.Aggregates.Books;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Replication;

public interface IReplicationManagerFactory {
    IReplicationManager Create(BookId bookId);
}
