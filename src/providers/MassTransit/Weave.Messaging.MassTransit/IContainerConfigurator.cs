using System;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContainerConfigurator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busConfigureFactory"></param>
        void Configure(Func<IBusControl> busConfigureFactory);

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        Func<IServiceFactory> ServiceFactoryProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        IContainerRegistration ContainerRegistration { get; }
    }
}
