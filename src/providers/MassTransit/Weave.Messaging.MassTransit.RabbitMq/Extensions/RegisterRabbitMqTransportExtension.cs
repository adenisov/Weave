using MassTransit;
using MassTransit.RabbitMqTransport;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.RabbitMq.Extensions
{
    internal sealed class RegisterRabbitMqTransportExtension : IEndpointExtension
    {
        private readonly RabbitMqHostSettings _rabbitMqHostSettings;

        public RegisterRabbitMqTransportExtension(RabbitMqHostSettings rabbitMqHostSettings)
        {
            _rabbitMqHostSettings = rabbitMqHostSettings;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += (sender, args) =>
                OnMessageBusConfiguring(args);
        }

        private void OnMessageBusConfiguring(MessageBusConfiguringEventArgs e) =>
            CreateHost((IRabbitMqBusFactoryConfigurator) e.Configurator, _rabbitMqHostSettings);

        private static void CreateHost(IRabbitMqBusFactoryConfigurator configurator, RabbitMqHostSettings hostSettings) =>
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
