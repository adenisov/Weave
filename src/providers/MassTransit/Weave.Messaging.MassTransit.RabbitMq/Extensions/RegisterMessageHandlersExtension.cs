using System;
using System.Collections.Generic;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Weave.Messaging.Core;
using Weave.Messaging.Debug.UseCases;

namespace Weave.Messaging.MassTransit.RabbitMq.Extensions
{
    public sealed class RegisterMessageHandlersExtension : IEndpointExtension
    {
        private sealed class RegisteredMessageHandler
        {
            public RegisteredMessageHandler(Type messageType, Type handlerType)
            {
                MessageType = messageType;
                HandlerType = handlerType;
            }

            public Type MessageType { get; }

            public Type HandlerType { get; }

            public override int GetHashCode() => HandlerType.GetHashCode();

            private bool Equals(RegisteredMessageHandler other) => HandlerType == other.HandlerType;

            public override bool Equals(object obj) =>
                ReferenceEquals(this, obj) || obj is RegisteredMessageHandler other && Equals(other);
        }

        private readonly RabbitMqHostSettings _rabbitMqHostSettings;
        private readonly IRabbitMqTopology _rabbitMqTopology;
        private readonly IRabbitMqTopologyFeaturesConfiguration _topologyFeaturesConfiguration;

        private readonly ICollection<RegisteredMessageHandler> _registeredQueryHandlers = new HashSet<RegisteredMessageHandler>();
        private readonly ICollection<RegisteredMessageHandler> _registeredCommandHandlers = new HashSet<RegisteredMessageHandler>();
        private readonly ICollection<RegisteredMessageHandler> _registeredEventHandlers = new HashSet<RegisteredMessageHandler>();

        private IRabbitMqHost _rabbitMqHost;
        private Func<IServiceFactory> _serviceFactoryProvider;
        private Action<RegistrationBuilder> _registrationBuilder;

        public RegisterMessageHandlersExtension(
            RabbitMqHostSettings rabbitMqHostSettings,
            IRabbitMqTopology rabbitMqTopology,
            IRabbitMqTopologyFeaturesConfiguration topologyFeaturesConfiguration)
        {
            _rabbitMqHostSettings = rabbitMqHostSettings;
            _rabbitMqTopology = rabbitMqTopology;
            _topologyFeaturesConfiguration = topologyFeaturesConfiguration;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.QueryHandlerRegistered += OnQueryHandlerRegistered;
            endpointLifecycle.CommandHandlerRegistered += OnCommandHandlerRegistered;
            endpointLifecycle.EventHandlerRegistered += OnEventHandlerRegistered;
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
            endpointLifecycle.ServiceFactoryConfigured += OnServiceLocatorConfigured;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            _registrationBuilder = e.Builder;
        }

        private void OnServiceLocatorConfigured(object sender, ServiceFactoryConfiguredEventArgs e)
        {
            _serviceFactoryProvider = e.ServiceFactoryProvider;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs messageBusConfiguringEventArgs)
        {
            var configurator = (IRabbitMqBusFactoryConfigurator) messageBusConfiguringEventArgs.Configurator;

            _rabbitMqHost = CreateHost(configurator);

            RegisterQueryHandlers(configurator, _registeredQueryHandlers);
            RegisterCommandHandlers(configurator, _registeredCommandHandlers);
            RegisterEventHandlers(configurator, _registeredEventHandlers);
        }

        private IRabbitMqHost CreateHost(IRabbitMqBusFactoryConfigurator configurator)
        {
            return configurator.Host(
                _rabbitMqHostSettings.Host,
                (ushort) _rabbitMqHostSettings.Port,
                _rabbitMqHostSettings.VirtualHost, c =>
                {
                    c.Username(_rabbitMqHostSettings.Username);
                    c.Password(_rabbitMqHostSettings.Password);
                });
        }

        private void RegisterQueryHandlers(IRabbitMqBusFactoryConfigurator configurator, IEnumerable<RegisteredMessageHandler> handlers)
        {
            foreach (var queryHandler in handlers)
            {
                var messageType = queryHandler.MessageType;

                var handlerAdapter = typeof(QueryHandlerConsumerAdapter<,,>).MakeGenericType(
                    queryHandler.HandlerType,
                    messageType,
                    messageType.GetResponseMessage());

                RegisterMessageHandler(
                    configurator,
                    queryHandler,
                    _rabbitMqTopology.GetRemoteMessageAddress(messageType),
                    _topologyFeaturesConfiguration.DirectInputSettings,
                    handlerAdapter);
            }
        }

        private void RegisterCommandHandlers(IRabbitMqBusFactoryConfigurator configurator, IEnumerable<RegisteredMessageHandler> handlers)
        {
            foreach (var commandHandler in handlers)
            {
                var messageType = commandHandler.MessageType;

                Type handlerAdapter;
                if (messageType == typeof(TestCommand))
                {
                    handlerAdapter = typeof(CommandHandlerConsumerAdapter<,>).MakeGenericType(
                        commandHandler.HandlerType,
                        messageType);
                }
                else
                {
                    handlerAdapter = typeof(CommandHandlerConsumerAdapter<,,>).MakeGenericType(
                        commandHandler.HandlerType,
                        messageType,
                        messageType.GetResponseMessage2());
                }

                RegisterMessageHandler(
                    configurator,
                    commandHandler,
                    _rabbitMqTopology.GetRemoteMessageAddress(messageType),
                    _topologyFeaturesConfiguration.DirectInputSettings,
                    handlerAdapter);
            }
        }

        private void RegisterEventHandlers(IRabbitMqBusFactoryConfigurator configurator, IEnumerable<RegisteredMessageHandler> handlers)
        {
            foreach (var eventHandler in handlers)
            {
                var messageType = eventHandler.MessageType;

                var handlerAdapter = typeof(EventHandlerConsumerAdapter<,>).MakeGenericType(
                    eventHandler.HandlerType,
                    messageType);

                RegisterMessageHandler(
                    configurator,
                    eventHandler,
                    _rabbitMqTopology.GetRemoteMessageAddress(messageType),
                    _topologyFeaturesConfiguration.PublishSettings,
                    handlerAdapter);
            }
        }

        private void RegisterMessageHandler(
            IRabbitMqBusFactoryConfigurator configurator,
            RegisteredMessageHandler handler,
            MessageAddress address,
            InputSettings inputSettings,
            Type handlerAdapterType)
        {
            var messageType = handler.MessageType;

            configurator.ReceiveEndpoint(
                _rabbitMqHost,
                _rabbitMqTopology.GetLocalInputQueueName(messageType),
                c =>
                {
                    c.AutoDelete = inputSettings.AutoDelete;
                    c.Durable = inputSettings.Durable;
                    c.BindMessageExchanges = inputSettings.BindMessageExchanges;
                    c.QueueExpiration = inputSettings.Ttl;
                    c.PrefetchCount = (ushort) inputSettings.PrefetchCount;

                    c.Bind(address.ExchangeName, e =>
                    {
                        e.AutoDelete = false;
                        e.Durable = true;
                        e.ExchangeType = address.ExchangeType.ToRabbitMqType();
                        e.RoutingKey = address.RoutingKey;
                    });

                    c.Consumer(handlerAdapterType, t => _serviceFactoryProvider().GetService(t));

                    _registrationBuilder(
                        RegistrationBuilder
                            .RegisterType(handlerAdapterType)
                            .AsSelf()
                            .PerNestedLifetimeScope());
                });
        }

        private void OnQueryHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs eventArgs)
        {
            _registeredQueryHandlers.Add(new RegisteredMessageHandler(eventArgs.MessageType,
                eventArgs.MessageHandlerType));
        }

        private void OnCommandHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs eventArgs)
        {
            _registeredCommandHandlers.Add(new RegisteredMessageHandler(eventArgs.MessageType,
                eventArgs.MessageHandlerType));
        }

        private void OnEventHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs eventArgs)
        {
            _registeredEventHandlers.Add(new RegisteredMessageHandler(eventArgs.MessageType,
                eventArgs.MessageHandlerType));
        }
    }
}
