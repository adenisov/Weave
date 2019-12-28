using System;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMassTransitEndpointLifecycle
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageHandlerRegisteredEventArgs> QueryHandlerRegistered;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageHandlerRegisteredEventArgs> CommandHandlerRegistered;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageHandlerRegisteredEventArgs> EventHandlerRegistered;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SagaRegisteredEventArgs> SagaRegistered; 

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ContainerRegisteredEventArgs> ContainerRegistered;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ServiceFactoryConfiguredEventArgs> ServiceFactoryConfigured; 

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageBusConfiguringEventArgs> MessageBusConfiguring;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageBusConfiguredEventArgs> MessageBusConfigured; 

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageBusStartedEventArgs> MessageBusStarted;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageBusStartingEventArgs> MessageBusStarting;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler MessageBusStopping;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler MessageBusStopped;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator"></param>
        void EmitMessageBusConfiguring(IBusFactoryConfigurator configurator);

        /// <summary>
        /// 
        /// </summary>
        void EmitMessageBusConfigured();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageHandlerType"></param>
        void EmitQueryHandlerRegistered(Type messageType, Type messageHandlerType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageHandlerType"></param>
        void EmitCommandHandlerRegistered(Type messageType, Type messageHandlerType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageHandlerType"></param>
        void EmitEventHandlerRegistered(Type messageType, Type messageHandlerType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sagaType"></param>
        void EmitSagaRegistered(Type sagaType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nativeBus"></param>
        /// <param name="bus"></param>
        void EmitMessageBusStarted(IBusControl nativeBus, IMassTransitMessageBus bus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="busControl"></param>
        void EmitMessageBusStarting(IBusControl busControl);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registar"></param>
        void EmitContainerProvided(Action<IContainerRegistar> registar);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceFactoryProvider"></param>
        void EmitServiceFactoryConfigured(Func<IServiceFactory> serviceFactoryProvider);

        /// <summary>
        /// 
        /// </summary>
        void EmitMessageBusStopping();

        /// <summary>
        /// 
        /// </summary>
        void EmitMessageBusStopped();
    }
}
