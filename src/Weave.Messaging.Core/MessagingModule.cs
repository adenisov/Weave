using System;
using System.Collections.Generic;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;
using Weave.Messaging.Core.Sagas;
using System.Reflection;
using JetBrains.Annotations;

namespace Weave.Messaging.Core
{
    public abstract class MessagingModule : IMessagingModule
    {
        private bool _isLoaded;
        
        private readonly Lazy<MessagingModuleBuilderImpl> _builderFactory =
            new Lazy<MessagingModuleBuilderImpl>(() => new MessagingModuleBuilderImpl());

        protected abstract void Load([NotNull] IMessagingModuleBuilder builder);

        public void RegisterQueryHandlers(IQueryBusConfiguration queryBusConfiguration)
        {
            LoadInternal();

            RegisterMessageHandler(
                typeof(IQueryBusConfiguration).GetMethod(nameof(IQueryBusConfiguration.RegisterQueryHandler)),
                _builderFactory.Value.QueryHandlerTypes,
                queryBusConfiguration);
        }

        public void RegisterCommandHandlers(ICommandBusConfiguration commandBusConfiguration)
        {
            LoadInternal();

            RegisterMessageHandler(
                typeof(ICommandBusConfiguration).GetMethod(nameof(ICommandBusConfiguration.RegisterCommandHandler)),
                _builderFactory.Value.CommandHandlerTypes,
                commandBusConfiguration);
        }

        public void RegisterEventHandlers(IEventBusConfiguration eventBusConfiguration)
        {
            LoadInternal();

            RegisterMessageHandler(
                typeof(IEventBusConfiguration).GetMethod(nameof(IEventBusConfiguration.RegisterEventHandler)),
                _builderFactory.Value.EventHandlerTypes,
                eventBusConfiguration);
        }

        public void RegisterSagas(ISagaConfiguration sagaConfiguration)
        {
            Load(_builderFactory.Value);

            foreach (var sagaType in _builderFactory.Value.SagaTypes)
            {
                // ReSharper disable once PossibleNullReferenceException
                typeof(ISagaConfiguration).GetMethod(nameof(ISagaConfiguration.RegisterSaga)).MakeGenericMethod(sagaType)
                    .Invoke(sagaConfiguration, null);
            }
        }

        private void LoadInternal()
        {
            if (_isLoaded)
            {
                return;
            }

            Load(_builderFactory.Value);
            _isLoaded = true;
        }

        private static void RegisterMessageHandler<TRegistry>(
            MethodInfo registrationMethod, IEnumerable<Type> handlerTypes, TRegistry registry)
            where TRegistry : class
        {
            foreach (var handlerType in handlerTypes)
            {
                registrationMethod.MakeGenericMethod(handlerType).Invoke(registry, null);
            }
        }
    }
}
