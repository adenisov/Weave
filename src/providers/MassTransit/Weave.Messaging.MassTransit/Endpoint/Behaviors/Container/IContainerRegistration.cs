using Automatonymous;
using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContainerRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        void Register<T>(IInstanceRegistrationSource<T> source)
            where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        void Register(RegistrationBuilder builder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerRegistrationBuilder"></param>
        /// <typeparam name="TConsumer"></typeparam>
        void Register<TConsumer>(IConsumerRegistrationBuilder<TConsumer> consumerRegistrationBuilder)
            where TConsumer : class, IConsumer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sagaRegistrationBuilder"></param>
        /// <typeparam name="TSagaInstance"></typeparam>
        void Register<TSagaInstance>(ISagaRegistrationBuilder<TSagaInstance> sagaRegistrationBuilder)
            where TSagaInstance : class, SagaStateMachineInstance;
    }
}
