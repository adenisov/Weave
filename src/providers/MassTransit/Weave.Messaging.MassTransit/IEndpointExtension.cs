using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    ///
    /// </summary>
    public interface IEndpointExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointLifecycle"></param>
        void Attach(IMassTransitEndpointLifecycle endpointLifecycle);
    }
}
