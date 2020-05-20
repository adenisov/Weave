using Weave.Messaging.Core;

namespace Weave.Messaging.MassTransit.Consumers.Behaviors
{
    internal sealed class IncomingMessage<TRequest> : IIncomingMessage<TRequest>
        where TRequest : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        public IncomingMessage(MessageHeaders headers, TRequest body)
        {
            Headers = headers;
            Body = body;
        }

        public MessageHeaders Headers { get; }

        /// <summary>
        /// 
        /// </summary>
        public TRequest Body { get; }
    }
}
