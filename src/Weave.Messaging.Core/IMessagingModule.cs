using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;
using Weave.Messaging.Core.Sagas;

namespace Weave.Messaging.Core
{
    /// <summary>
    /// Defines a messaging module.
    /// </summary>
    public interface IMessagingModule
    {
        /// <summary>
        /// Registers the query configuration.
        /// </summary>
        /// <param name="queryBusConfiguration"></param>
        void RegisterQueryHandlers(IQueryBusConfiguration queryBusConfiguration);

        /// <summary>
        /// Registers the command configuration.
        /// </summary>
        /// <param name="commandBusConfiguration"></param>
        void RegisterCommandHandlers(ICommandBusConfiguration commandBusConfiguration);

        /// <summary>
        /// Registers the event configuration.
        /// </summary>
        /// <param name="eventBusConfiguration"></param>
        void RegisterEventHandlers(IEventBusConfiguration eventBusConfiguration);

        /// <summary>
        /// Registers the saga configuration.
        /// </summary>
        /// <param name="sagaConfiguration"></param>
        void RegisterSagas(ISagaConfiguration sagaConfiguration);
    }
}
