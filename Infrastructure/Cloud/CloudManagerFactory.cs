using Application.Contracts.Cloud;
using Application.Contracts.Event;
using Domain.Aggregates.Books;
using Infrastructure.Event;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cloud;
public class CloudManagerFactory(EventAggregator eventAggregator, IEventsClient eventsClient, ICloudEventDispatcher cloudEventDispatcher, ILoggerFactory loggerFactory) : ICloudManagerFactory {
    private readonly IEventsClient _eventsClient = eventsClient;
    private readonly ICloudEventDispatcher _cloudEventDispatcher = cloudEventDispatcher;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public ICloudManager Create(Guid instanceId, BookId bookId) {
        return new CloudManager(instanceId, bookId, eventAggregator, _eventsClient, _cloudEventDispatcher, _loggerFactory.CreateLogger<CloudManager>());
    }
}
