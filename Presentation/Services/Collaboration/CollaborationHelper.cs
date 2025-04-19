using Application.Services.Messages;
using Application.Services.Pages;
using Domain.Entities.Books;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Presentation.Services.Collaboration;
public sealed class CollaborationHelper {
    private readonly IMessageManagerFactory _messageManagerFactory = App.Current.ServiceProvider.GetRequiredService<IMessageManagerFactory>();

    public Func<Task> Use(BookId bookId) {
        var messageManager = _messageManagerFactory.Create(bookId);

        return async () => {

            try {
                await messageManager.FlushAsync();

                var events = await messageManager.GetEventsAsync();
                if (events.Any())
                    Debug.WriteLine($"Events: {events.Count}");
                else
                    Debug.WriteLine("No events");
            } catch (Exception e) {

                throw;
            }
        };
    }
}
