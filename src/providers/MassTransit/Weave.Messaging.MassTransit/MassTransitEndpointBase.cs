#pragma warning disable 618
//

using System;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;
using Weave.Messaging.Core.Sagas;
using Weave.Messaging.MassTransit.Endpoint.Behaviors;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit
{
    public abstract class MassTransitEndpointBase : IMassTransitEndpoint
    {
        private readonly IMassTransitEndpointLifecycle _lifecycle;
        private readonly IEndpointExtension[] _extensions;

        private IBusControl _busControl;
        private IMassTransitMessageBus _messageBus;
        private IContainerConfigurator _containerConfigurator;
        private Func<IServiceFactory> _serviceFactoryProvider;

        protected MassTransitEndpointBase(params IEndpointExtension[] extensions)
        {
            _extensions = extensions;
            _lifecycle = new MassTransitEndpointLifecycle();

            AttachBehaviors();
            AttachExtensions();
        }

        protected abstract Func<IBusControl> GetConfigureFactory(Action<IBusFactoryConfigurator> configurator);

        public void RegisterQueryHandler<THandler>() where THandler : IQueryHandler
        {
            var queryMessageType = typeof(THandler).GetQueryMessageType();
            _lifecycle.EmitQueryHandlerRegistered(queryMessageType, typeof(THandler));
        }

        public void RegisterCommandHandler<THandler>() where THandler : ICommandHandler
        {
            var commandMessageType = typeof(THandler).GetCommandMessageType();
            _lifecycle.EmitCommandHandlerRegistered(commandMessageType, typeof(THandler));
        }

        public void RegisterEventHandler<THandler>() where THandler : IEventHandler
        {
            var eventMessageType = typeof(THandler).GetEventMessageType();
            _lifecycle.EmitEventHandlerRegistered(eventMessageType, typeof(THandler));
        }

        public void RegisterSaga<TSaga>() where TSaga : ISaga
        {
            _lifecycle.EmitSagaRegistered(typeof(TSaga));
        }

        public void RegisterMessagingModule<TModule>(TModule messagingModule)
            where TModule : IMessagingModule, new()
        {
            var module = new TModule();
            {
                module.RegisterQueryHandlers(this);
                module.RegisterCommandHandlers(this);
                module.RegisterEventHandlers(this);
                module.RegisterSagas(this);
            }
        }

        public void ConfigureContainer(IContainerConfigurator configurator)
        {
            _containerConfigurator = configurator;

            _serviceFactoryProvider = _containerConfigurator.ServiceFactoryProvider;
            _lifecycle.EmitServiceFactoryConfigured(_serviceFactoryProvider);

            var containerRegistration = _containerConfigurator.ContainerRegistration;
            _lifecycle.EmitContainerProvided(_ => _.Register(containerRegistration));
        }

        public void Configure(Action<IBusFactoryConfigurator> postConfigurationAction = null)
        {
            void ConfigurationAction(IBusFactoryConfigurator configurator)
            {
                _lifecycle.EmitMessageBusConfiguring(configurator);
                postConfigurationAction?.Invoke(configurator);
                _lifecycle.EmitMessageBusConfigured();
            }

            _containerConfigurator.Configure(GetConfigureFactory(ConfigurationAction));

            /*
            using (var serviceFactory = _serviceFactoryProvider())
            {
                _busControl = serviceFactory.GetService<IBusControl>();
                _lifecycle.EmitMessageBusConfigured(_busControl);
            }
            */
        }

        public IMassTransitMessageBus CreateMessageBus()
        {
            if (_messageBus != null)
            {
                throw new InvalidOperationException("message bus has been already created.");
            }

            var serviceFactory = _serviceFactoryProvider();
            _busControl = serviceFactory.GetService<IBusControl>();

            if (_busControl == null)
            {
                throw new InvalidOperationException("busControl wasn't properly configured. forgot to call .Configure()?");
            }

            _lifecycle.EmitMessageBusStarting(_busControl);
            _busControl.Start();

            _messageBus = serviceFactory.GetService<IMassTransitMessageBus>();
            _lifecycle.EmitMessageBusStarted(_busControl, _messageBus);

            return _messageBus;
        }

        private void AttachBehaviors()
        {
            foreach (var endpointBehavior in _defaultBehaviorSet)
            {
                endpointBehavior.Attach(_lifecycle);
            }
        }

        private void AttachExtensions()
        {
            foreach (var extension in _extensions)
            {
                extension.Attach(_lifecycle);
            }
        }

        private readonly IEndpointBehavior[] _defaultBehaviorSet =
        {
            new RegisterMessageHandlersInContainer(),
            new RegisterEntityFormatter(() => GlobalEntityNameFormatter.Default),
            new ConfigureParallelOptionsBehavior(),
            new ConfigureSerializationBehavior(),
        };

        public void Dispose()
        {
            _lifecycle.EmitMessageBusStopping();

            _messageBus?.Dispose();
            _busControl?.Stop();

            _lifecycle.EmitMessageBusStopped();
        }
    }
}
