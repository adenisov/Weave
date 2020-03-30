using System;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.RabbitMq.Extensions
{
    public sealed class RegisterRabbitMqTransportExtension : IEndpointExtension
    {
        private readonly RabbitMqHostSettings _rabbitMqHostSettings;

        public RegisterRabbitMqTransportExtension(RabbitMqHostSettings rabbitMqHostSettings)
        {
            _rabbitMqHostSettings = rabbitMqHostSettings;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += (sender, args) =>
                OnMessageBusConfiguring(host =>
                    endpointLifecycle.EmitMessageBusTransportConfigured(host, args.Configurator), args);
        }

        private void OnMessageBusConfiguring(Action<IHost> hostEmitter, MessageBusConfiguringEventArgs e) =>
            hostEmitter(CreateHost((IRabbitMqBusFactoryConfigurator) e.Configurator));

        private IRabbitMqHost CreateHost(IRabbitMqBusFactoryConfigurator configurator) =>
            configurator.Host(
                _rabbitMqHostSettings.Host,
                (ushort) _rabbitMqHostSettings.Port,
                _rabbitMqHostSettings.VirtualHost, c =>
                {
                    c.Username(_rabbitMqHostSettings.Username);
                    c.Password(_rabbitMqHostSettings.Password);
                });
    }
}
