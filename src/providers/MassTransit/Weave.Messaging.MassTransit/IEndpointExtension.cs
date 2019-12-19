namespace Weave.Messaging.MassTransit
{
    /// <summary>
    ///
    /// </summary>
    public interface IEndpointExtension
    {
        void Attach(IMassTransitEndpointLifecycle endpointLifecycle);
    }
}
