using Weave.Messaging.Core.Sagas;

namespace Weave.Messaging.Core
{
    /// <summary>
    /// Defines a low-level module builder.
    /// </summary>
    public interface IMessagingModuleBuilder
    {
        /// <summary>
        /// Registers a specific message handler.
        /// </summary>
        /// <typeparam name="THandler">Type of the message handler.</typeparam>
        void WithHandler<THandler>() where THandler : class;

        /// <summary>
        /// Registers a saga.
        /// </summary>
        /// <typeparam name="TSaga">Saga type</typeparam>
#pragma warning disable 618
        void WithSaga<TSaga>() where TSaga : ISaga;
#pragma warning restore 618
    }
}
