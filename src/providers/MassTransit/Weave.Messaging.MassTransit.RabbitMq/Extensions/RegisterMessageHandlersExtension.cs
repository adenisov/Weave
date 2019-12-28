using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Weave.Messaging.Core;
using Weave.Messaging.Debug.UseCases.Saga;
using Weave.Messaging.MassTransit.Consumers;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

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
        private readonly ICollection<Type> _registeredSagas = new HashSet<Type>();

        private IRabbitMqHost _host;
        private IRabbitMqBusFactoryConfigurator _configurator;
        private Action<IContainerRegistar> _containerRegistar;

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
            endpointLifecycle.SagaRegistered += OnSagaRegistered;
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            _containerRegistar = e.Registar;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            _configurator = (IRabbitMqBusFactoryConfigurator) e.Configurator;
            _host = CreateHost(_configurator);

            RegisterQueryHandlers(_registeredQueryHandlers);
            RegisterCommandHandlers(_registeredCommandHandlers);
            RegisterEventHandlers(_registeredEventHandlers);
            RegisterSagas(_registeredSagas);
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

        private void RegisterQueryHandlers(IEnumerable<RegisteredMessageHandler> handlers)
        {
            foreach (var queryHandler in handlers)
            {
                var messageType = queryHandler.MessageType;

                var handlerAdapterType = typeof(QueryHandlerConsumerAdapter<,,>).MakeGenericType(
                    queryHandler.HandlerType,
                    messageType,
                    messageType.GetResponseMessage());

                RegisterMessageHandler(handlerAdapterType, queryHandler, _topologyFeaturesConfiguration.DirectInputSettings);
            }
        }

        private void RegisterCommandHandlers(IEnumerable<RegisteredMessageHandler> handlers)
        {
            foreach (var commandHandler in handlers)
            {
                var messageType = commandHandler.MessageType;

                Type handlerAdapterType;
                if (messageType.HasResponseMessage())
                {
                    handlerAdapterType = typeof(CommandHandlerConsumerAdapter<,>).MakeGenericType(
                        commandHandler.HandlerType,
                        messageType);
                }
                else
                {
                    handlerAdapterType = typeof(CommandHandlerConsumerAdapter<,,>).MakeGenericType(
                        commandHandler.HandlerType,
                        messageType,
                        messageType.GetResponseMessage2());
                }

                RegisterMessageHandler(handlerAdapterType, commandHandler, _topologyFeaturesConfiguration.DirectInputSettings);
            }
        }

        private void RegisterEventHandlers(IEnumerable<RegisteredMessageHandler> handlers)
        {
            foreach (var eventHandler in handlers)
            {
                var messageType = eventHandler.MessageType;

                var handlerAdapterType = typeof(EventHandlerConsumerAdapter<,>).MakeGenericType(
                    eventHandler.HandlerType,
                    messageType);

                RegisterMessageHandler(handlerAdapterType, eventHandler, _topologyFeaturesConfiguration.PublishSettings);
            }
        }

        private void RegisterSagas(IEnumerable<Type> sagas)
        {
            foreach (var sagaType in sagas)
            {
                _configurator.ReceiveEndpoint(
                    _host,
                    _rabbitMqTopology.GetLocalInputQueueName(sagaType),
                    c =>
                    {
                        _containerRegistar(
                            SagaRegistrationFactory
                                .ForSaga<OrderSagaData>()
                                .WithReceiveEndpointConfiguration(c));
                    });
            }
        }

        private void RegisterMessageHandler(Type handlerType, RegisteredMessageHandler handler, InputSettings inputSettings)
        {
            // ReSharper disable once PossibleNullReferenceException
            var method = typeof(RegisterMessageHandlersExtension)
                .GetMethod(nameof(RegisterMessageConsumer), BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(handlerType);

            method.Invoke(this, new object[] { handler, inputSettings });
        }

        [UsedImplicitly]
        private void RegisterMessageConsumer<TConsumer>(RegisteredMessageHandler handler, InputSettings inputSettings)
            where TConsumer : class, IConsumer
        {
            var messageType = handler.MessageType;

            _configurator.ReceiveEndpoint(
                _host,
                _rabbitMqTopology.GetLocalInputQueueName(messageType),
                c =>
                {
                    c.AutoDelete = inputSettings.AutoDelete;
                    c.Durable = inputSettings.Durable;
                    c.BindMessageExchanges = inputSettings.BindMessageExchanges;
                    c.QueueExpiration = inputSettings.Ttl;
                    c.PrefetchCount = (ushort) inputSettings.PrefetchCount;

                    // ReSharper disable once HeapView.ClosureAllocation
                    var address = _rabbitMqTopology.GetRemoteMessageAddress(messageType);
                    c.Bind(address.ExchangeName, e =>
                    {
                        e.AutoDelete = false;
                        e.Durable = true;
                        e.ExchangeType = address.ExchangeType.ToRabbitMqType();
                        e.RoutingKey = address.RoutingKey;
                    });

                    _containerRegistar(
                        ConsumerRegistrationFactory
                            .ForConsumer<TConsumer>()
                            .WithReceiveEndpointConfiguration(c));
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

        private void OnSagaRegistered(object sender, SagaRegisteredEventArgs e)
        {
            _registeredSagas.Add(e.SagaType);
        }
    }
}
