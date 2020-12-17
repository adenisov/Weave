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
        private readonly IContainerConfigurator _containerConfigurator;

        private IBusControl _busControl;
        private IMassTransitMessageBus _messageBus;
        private IContainerRegistration _containerRegistration;
        private Func<ConsumeContext, IServiceFactory> _serviceFactoryProvider;

        protected MassTransitEndpointBase(IContainerConfigurator containerConfigurator, params IEndpointExtension[] extensions)
        {
            _containerConfigurator = containerConfigurator;
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
            where TModule : IMessagingModule
        {
            messagingModule.RegisterQueryHandlers(this);
            messagingModule.RegisterCommandHandlers(this);
            messagingModule.RegisterEventHandlers(this);
            messagingModule.RegisterSagas(this);
        }

        public void RegisterMessagingModule<TModule>()
            where TModule : IMessagingModule, new()
        {
            RegisterMessagingModule(new TModule());
        }

        public void Configure(Action<IBusFactoryConfigurator> preConfigure = null, Action<IBusFactoryConfigurator> postConfigure = null)
        {
            void ConfigurationAction(IBusFactoryConfigurator configurator)
            {
                preConfigure?.Invoke(configurator);
                _lifecycle.EmitMessageBusConfiguring(configurator);
                postConfigure?.Invoke(configurator);
                _lifecycle.EmitMessageBusConfigured();
            }

            ConfigureContainer();
            _containerConfigurator.Configure(GetConfigureFactory(ConfigurationAction));
        }

        public IMassTransitMessageBus CreateMessageBus(TimeSpan timeout = default)
        {
            if (_messageBus != null)
            {
                throw new InvalidOperationException("message bus has been already created.");
            }

            var serviceFactory = _serviceFactoryProvider(null);
            _busControl = serviceFactory.GetService<IBusControl>();

            if (_busControl == null)
            {
                throw new InvalidOperationException($"busControl wasn't properly configured. forgot to call {nameof(Configure)}?");
            }

            _lifecycle.EmitMessageBusStarting(_busControl);
            if (timeout == default)
            {
                _busControl.Start();
            }
            else
            {
                _busControl.Start(timeout);
            }

            _messageBus = serviceFactory.GetService<IMassTransitMessageBus>();
            _lifecycle.EmitMessageBusStarted(_busControl, _messageBus);

            return _messageBus;
        }

        private void ConfigureContainer()
        {
            _serviceFactoryProvider = _containerConfigurator.ServiceFactoryProvider;
            _lifecycle.EmitServiceFactoryConfigured(_serviceFactoryProvider);

            _containerRegistration = _containerConfigurator.ContainerRegistration;
            _lifecycle.EmitContainerProvided(_ => _.Register(_containerRegistration));
        }

        private void AttachBehaviors()
        {
            foreach (var behavior in _defaultBehaviorSet)
            {
                behavior.Attach(_lifecycle);
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
            new ProvideAccessToServiceLocatorBehavior(),
            new RegisterMessageHandlersInContainer(),
            new RegisterSagasInContainer(),
            new RegisterEntityFormatter(),
            new ConfigureParallelOptionsBehavior(),
            new ConfigureSerializationBehavior(),
            new ExecuteHandlersInTransactionBehavior(),
            // new MessageRetryBehavior(),
            // new MessageRedeliveryBehavior(),
            new RegisterMessageBusBehaviors(new ConfigurePredefinedMessageHeaders()),
        };

        private void ShutdownExtensions()
        {
            foreach (var endpointExtension in _extensions)
            {
                endpointExtension.Dispose();
            }
        }

        private void ShutdownMessageBus()
        {
            _lifecycle.EmitMessageBusStopping();

            ShutdownExtensions();
            _busControl?.Stop(TimeSpan.FromSeconds(10));

            _lifecycle.EmitMessageBusStopped();
            _containerConfigurator?.Dispose();
        }

        public void Dispose()
        {
            ShutdownMessageBus();
            GC.SuppressFinalize(this);
        }
    }
}
