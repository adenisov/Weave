using Weave.Messaging.Core;

namespace Weave.Messaging.MassTransit.Consumers.Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IIncomingMessage<out TRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        MessageHeaders Headers { get; }

        /// <summary>
        /// 
        /// </summary>
        TRequest Body { get; }
    }
}
