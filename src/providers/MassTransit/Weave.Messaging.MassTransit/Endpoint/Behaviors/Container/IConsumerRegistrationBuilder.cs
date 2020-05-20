using System;
using MassTransit;
using MassTransit.ConsumeConfigurators;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TConsumer"></typeparam>
    public interface IConsumerRegistrationBuilder<TConsumer> : IReceiveEndpointConfiguration, IContainerRegistar
        where TConsumer : class, IConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator"></param>
        /// <typeparam name="TConfigurator"></typeparam>
        /// <returns></returns>
        IConsumerRegistrationBuilder<TConsumer> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
            where TConfigurator : class, IReceiveEndpointConfigurator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator"></param>
        /// <returns></returns>
        IConsumerRegistrationBuilder<TConsumer> WithConsumerConfiguration(Action<IConsumerConfigurator<TConsumer>> configurator);

        /// <summary>
        /// 
        /// </summary>
        Action<IConsumerConfigurator<TConsumer>> ConsumerConfigurator { get; }
    }
}
