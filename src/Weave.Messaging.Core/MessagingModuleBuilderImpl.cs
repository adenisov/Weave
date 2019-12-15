using System;
using System.Collections.Generic;
using Weave.Messaging.Core.Sagas;

namespace Weave.Messaging.Core
{
    internal sealed class MessagingModuleBuilderImpl : IMessagingModuleBuilder
    {
        public ICollection<Type> QueryHandlerTypes { get; } = new HashSet<Type>();

        public ICollection<Type> CommandHandlerTypes { get; } = new HashSet<Type>();

        public ICollection<Type> EventHandlerTypes { get; } = new HashSet<Type>();

        public ICollection<Type> SagaTypes { get; } = new HashSet<Type>();

        public void WithHandler<THandler>() where THandler : class
        {
            var handlerType = typeof(THandler);

            if (handlerType.IsQueryHandlerType())
            {
                QueryHandlerTypes.Add(handlerType);
            }
            else if (handlerType.IsCommandHandlerType())
            {
                CommandHandlerTypes.Add(handlerType);
            }
            else if (handlerType.IsEventHandlerType())
            {
                EventHandlerTypes.Add(handlerType);
            }
            else if (handlerType.IsSagaType())
            {
                SagaTypes.Add(handlerType);
            }
            else
            {
                throw new InvalidOperationException("");
            }
        }

#pragma warning disable 618
        public void WithSaga<TSaga>() where TSaga : ISaga
#pragma warning restore 618
        {
            // ToDo: register sagas
        }
    }
}