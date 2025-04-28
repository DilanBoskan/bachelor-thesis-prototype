using CommunityToolkit.Mvvm.Messaging.Messages;
using Domain.Aggregates.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Messenges;
public class RequestLoadedPagesMessage : RequestMessage<HashSet<PageId>>;
