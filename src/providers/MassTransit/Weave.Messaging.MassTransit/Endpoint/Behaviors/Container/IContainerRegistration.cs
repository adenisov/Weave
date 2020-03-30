using Automatonymous;
using MassTransit;
using ISaga = MassTransit.Saga.ISaga;

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
        /// <param name="builder"></param>
        /// <typeparam name="TConsumer"></typeparam>
        void Register<TConsumer>(IConsumerRegistrationBuilder<TConsumer> builder)
            where TConsumer : class, IConsumer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TSagaInstance"></typeparam>
        void Register<TSagaInstance>(IStateMachineSagaRegistrationBuilder<TSagaInstance> builder) 
            where TSagaInstance : class, SagaStateMachineInstance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TSaga"></typeparam>
        void Register<TSaga>(ISagaRegistrationBuilder<TSaga> builder)
            where TSaga : class, ISaga;
    }
}
