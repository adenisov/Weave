using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;
using Weave.Messaging.Core.Sagas;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMassTransitMessageHandlerConfiguration :
        IQueryBusConfiguration,
        ICommandBusConfiguration,
        IEventBusConfiguration,
        ISagaConfiguration
    {
    }
}
