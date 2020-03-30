using System;
using System.Threading.Tasks;

namespace Weave.Messaging.Core.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        Task Publish<TEvent>(IEventMessage<TEvent> @event, Action<EventConfiguration> configuration = null)
            where TEvent : class, IEventMessage<TEvent>;
    }
}
