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
        void Attach(IMassTransitEndpointLifecycle endpointLifecycle);
    }
}
