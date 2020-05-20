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
            hostEmitter(CreateHost((IRabbitMqBusFactoryConfigurator) e.Configurator, _rabbitMqHostSettings));

        private static IRabbitMqHost CreateHost(IRabbitMqBusFactoryConfigurator configurator, RabbitMqHostSettings hostSettings) =>
            configurator.Host(
                hostSettings.Host,
                (ushort) hostSettings.Port,
                hostSettings.VirtualHost, c =>
                {
                    c.Username(hostSettings.Username);
                    c.Password(hostSettings.Password);
                });

        public void Dispose()
        {
        }
    }
}
