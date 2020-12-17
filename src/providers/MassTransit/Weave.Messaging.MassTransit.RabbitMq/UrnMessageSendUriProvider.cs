using System;
using MassTransit;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    public sealed class UrnMessageSendUriProvider : IMessageSendUriProvider
    {
        private const string BaseUrl = "rabbitmq://localhost/";

        public Uri GetSendUri<TMessage>() => new Uri($"{BaseUrl}{MessageUrn.ForType<TMessage>()}");
    }
}
