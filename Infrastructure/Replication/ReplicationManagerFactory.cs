using Application.Contracts.Event;
using Application.Contracts.Replication;
using Domain.Aggregates.Books;
using Infrastructure.Event;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Replication;
public class ReplicationManagerFactory(EventAggregator eventAggregator, IEventsClient eventsClient, ICloudEventDispatcher cloudEventDispatcher, ILoggerFactory loggerFactory) : IReplicationManagerFactory {
    private readonly IEventsClient _eventsClient = eventsClient;
    private readonly ICloudEventDispatcher _cloudEventDispatcher = cloudEventDispatcher;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public IReplicationManager Create(Guid instanceId, BookId bookId) {
        return new ReplicationManager(instanceId, bookId, eventAggregator, _eventsClient, _cloudEventDispatcher, _loggerFactory.CreateLogger<ReplicationManager>());
    }
}
