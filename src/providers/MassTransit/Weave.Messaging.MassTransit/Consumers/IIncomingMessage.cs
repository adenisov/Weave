using Weave.Messaging.Core;

namespace Weave.Messaging.MassTransit.Consumers
{
    public interface IIncomingMessage<out TRequest>
    {
        MessageHeaders Headers { get; }

        TRequest Body { get; }
    }
}
