using System;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    public interface IRabbitMqTopology
    {
        string GetLocalInputQueueName(Type messageType);

        string GetLocalInputQueueName<TMessage>();

        MessageAddress GetRemoteMessageAddress(Type messageType);

        MessageAddress GetRemoteMessageAddress<TMessage>();
    }
}