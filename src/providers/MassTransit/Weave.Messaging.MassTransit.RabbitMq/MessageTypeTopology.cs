using System;
using MassTransit;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    internal sealed class MessageTypeTopology : IRabbitMqTopology
    {
        private readonly string _applicationName;

        public MessageTypeTopology(string applicationName)
        {
            _applicationName = applicationName;
        }

        public string GetLocalInputQueueName(Type messageType) => $"{_applicationName}_{messageType.FullName}";

        public string GetLocalInputQueueName<TMessage>() => GetLocalInputQueueName(typeof(TMessage));

        public MessageAddress GetRemoteMessageAddress(Type messageType) =>
            new MessageAddress
            {
                ExchangeName = MessageUrn.ForType(messageType).ToString(),
                ExchangeType = ExchangeType.Fanout
            };

        public MessageAddress GetRemoteMessageAddress<TMessage>() => GetRemoteMessageAddress(typeof(TMessage));
    }
}
