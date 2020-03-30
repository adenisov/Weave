using MassTransit;
using MassTransit.Saga;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSaga"></typeparam>
    public interface ISagaRegistrationBuilder<TSaga> : IReceiveEndpointConfiguration, IContainerRegistar
        where TSaga : ISaga
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator"></param>
        /// <typeparam name="TConfigurator"></typeparam>
        /// <returns></returns>
        ISagaRegistrationBuilder<TSaga> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
            where TConfigurator : class, IReceiveEndpointConfigurator;
    }
}
