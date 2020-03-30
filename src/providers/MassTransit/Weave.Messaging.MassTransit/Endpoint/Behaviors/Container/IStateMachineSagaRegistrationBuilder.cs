using Automatonymous;
using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSagaInstance"></typeparam>
    public interface IStateMachineSagaRegistrationBuilder<TSagaInstance> : IReceiveEndpointConfiguration, IContainerRegistar
        where TSagaInstance : class, SagaStateMachineInstance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator"></param>
        /// <typeparam name="TConfigurator"></typeparam>
        /// <returns></returns>
        IStateMachineSagaRegistrationBuilder<TSagaInstance> WithReceiveEndpointConfiguration<TConfigurator>(
            TConfigurator configurator)
            where TConfigurator : class, IReceiveEndpointConfigurator;
    }
}
