using System;
using System.Threading;
using System.Threading.Tasks;

namespace Weave.Messaging.Core.Events
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("This is just a marker interface. It shouldn't be implemented.")]
    public interface IEventHandler : IMessageHandler
    {
    }

    /// <summary>
    /// 
    /// </summary>
#pragma warning disable 618
    public interface IEventHandler<in TEvent> : IEventHandler
#pragma warning restore 618
        where TEvent : IEventMessage<TEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task HandleAsync(TEvent @event, CancellationToken ct);
    }
}
