using JetBrains.Annotations;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    ///
    /// </summary>
    public interface IEndpointBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointLifecycle"></param>
        void Attach([NotNull] IMassTransitEndpointLifecycle endpointLifecycle);
    }
}
