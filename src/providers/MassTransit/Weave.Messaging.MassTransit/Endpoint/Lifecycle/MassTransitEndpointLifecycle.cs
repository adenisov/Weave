using System;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle
{
    internal sealed class MassTransitEndpointLifecycle : IMassTransitEndpointLifecycle
    {
        public event EventHandler<ContainerRegisteredEventArgs> ContainerRegistered;
        public event EventHandler<ServiceFactoryConfiguredEventArgs> ServiceFactoryConfigured;
        public event EventHandler<MessageBusConfiguringEventArgs> MessageBusConfiguring;
        public event EventHandler<MessageBusTransportConfiguredEventArgs> MessageBusTransportConfigured;
        public event EventHandler<MessageBusConfiguredEventArgs> MessageBusConfigured;
        public event EventHandler<MessageBusStartedEventArgs> MessageBusStarted;
        public event EventHandler<MessageHandlerRegisteredEventArgs> QueryHandlerRegistered;
        public event EventHandler<MessageHandlerRegisteredEventArgs> CommandHandlerRegistered;
        public event EventHandler<MessageHandlerRegisteredEventArgs> EventHandlerRegistered;
        public event EventHandler<SagaRegisteredEventArgs> SagaRegistered;
        public event EventHandler<MessageBusStartingEventArgs> MessageBusStarting;
        public event EventHandler MessageBusStopping;
        public event EventHandler MessageBusStopped;

        public void EmitMessageBusConfiguring(IBusFactoryConfigurator configurator)
        {
            MessageBusConfiguring?.Invoke(this, new MessageBusConfiguringEventArgs(configurator));
        }

        public void EmitMessageBusTransportConfigured(IHost host, IBusFactoryConfigurator configurator)
        {
            MessageBusTransportConfigured?.Invoke(this, new MessageBusTransportConfiguredEventArgs(host, configurator));
        }

        public void EmitMessageBusConfigured()
        {
            MessageBusConfigured?.Invoke(this, new MessageBusConfiguredEventArgs());
        }

        public void EmitQueryHandlerRegistered(Type messageType, Type messageHandlerType)
        {
            QueryHandlerRegistered?.Invoke(this, new MessageHandlerRegisteredEventArgs(messageType, messageHandlerType));
        }

        public void EmitCommandHandlerRegistered(Type messageType, Type messageHandlerType)
        {
            CommandHandlerRegistered?.Invoke(this, new MessageHandlerRegisteredEventArgs(messageType, messageHandlerType));
        }

        public void EmitEventHandlerRegistered(Type messageType, Type messageHandlerType)
        {
            EventHandlerRegistered?.Invoke(this, new MessageHandlerRegisteredEventArgs(messageType, messageHandlerType));
        }

        public void EmitSagaRegistered(Type sagaType)
        {
            SagaRegistered?.Invoke(this, new SagaRegisteredEventArgs(sagaType));
        }

        public void EmitMessageBusStarted(IBusControl nativeBus, IMassTransitMessageBus bus)
        {
            MessageBusStarted?.Invoke(this, new MessageBusStartedEventArgs(nativeBus, bus));
        }

        public void EmitMessageBusStarting(IBusControl busControl)
        {
            MessageBusStarting?.Invoke(this, new MessageBusStartingEventArgs(busControl));
        }

        public void EmitContainerProvided(ContainerRegistar registar)
        {
            ContainerRegistered?.Invoke(this, new ContainerRegisteredEventArgs(registar));
        }

        public void EmitServiceFactoryConfigured(Func<IServiceFactory> serviceFactoryProvider)
        {
            ServiceFactoryConfigured?.Invoke(this, new ServiceFactoryConfiguredEventArgs(serviceFactoryProvider));
        }

        public void EmitMessageBusStopping()
        {
            MessageBusStopping?.Invoke(this, EventArgs.Empty);
        }

        public void EmitMessageBusStopped()
        {
            MessageBusStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}
