using System;
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
        /// <param name="lifecycle"></param>
        void Configure(IMassTransitEndpointLifecycle lifecycle);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Func<IServiceFactory> GetServiceFactoryProvider();
    }
}
