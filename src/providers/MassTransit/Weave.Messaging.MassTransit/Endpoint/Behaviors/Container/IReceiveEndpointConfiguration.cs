using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReceiveEndpointConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        IReceiveEndpointConfigurator Configurator { get; }
    }
}
