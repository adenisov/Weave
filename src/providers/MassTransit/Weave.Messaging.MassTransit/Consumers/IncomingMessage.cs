using Weave.Messaging.Core;

namespace Weave.Messaging.MassTransit.Consumers
{
    internal sealed class IncomingMessage<TRequest> : IIncomingMessage<TRequest>
    {
        public IncomingMessage(TRequest body, MessageHeaders headers)
        {
            Headers = headers;
            Body = body;
        }

        public MessageHeaders Headers { get; }

        public TRequest Body { get; }
    }
}
