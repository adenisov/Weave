using System;
using JetBrains.Annotations;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContainerConfigurator : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busConfigureFactory"></param>
        void Configure([NotNull] Func<IBusControl> busConfigureFactory);

        /// <summary>
        /// 
        /// </summary>
        Func<ConsumeContext, IServiceFactory> ServiceFactoryProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        IContainerRegistration ContainerRegistration { get; }
    }
}
