using System;
using Weave.Messaging.Core;

namespace Weave.Messaging.MassTransit.Consumers.Behaviors
{
    public sealed class IncomingMessage<TRequest> : IIncomingMessage<TRequest>
        where TRequest : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        public IncomingMessage(MessageHeaders headers, TRequest body)
        {
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            Body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public MessageHeaders Headers { get; }

        /// <summary>
        /// 
        /// </summary>
        public TRequest Body { get; }
    }
}
