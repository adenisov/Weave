using System;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    ///
    /// </summary>
    public interface IEndpointExtension : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointLifecycle"></param>
        void Attach(IMassTransitEndpointLifecycle endpointLifecycle);
    }
}
