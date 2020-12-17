using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using GreenPipes;
using MassTransit;
using Weave.Messaging.MassTransit.Consumers;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class ExecuteHandlersInTransactionBehavior : DistinctiveConsumerConfigurationObserver, IEndpointBehavior
    {
        private readonly IsolationLevel _defaultIsolationLevel;
        private readonly TimeSpan _defaultTransactionTimeout;

        private readonly IDictionary<Type, HandlerTransactionOptions> _handlerTransactionOptions =
            new Dictionary<Type, HandlerTransactionOptions>();

        public ExecuteHandlersInTransactionBehavior(
            IsolationLevel defaultIsolationLevel = IsolationLevel.ReadCommitted,
            TimeSpan defaultTransactionTimeout = default)
        {
            _defaultIsolationLevel = defaultIsolationLevel;
            _defaultTransactionTimeout = defaultTransactionTimeout;
        }

        private sealed class HandlerTransactionOptions
        {
            public IsolationLevel IsolationLevel { get; set; }

            public TimeSpan Timeout { get; set; }

            public bool Suppress { get; set; }
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
            endpointLifecycle.MessageHandlerRegistered += OnMessageHandlerRegistered;
        }

        protected override void OnConsumerConfigured<TConsumer, TContext>(IPipeConfigurator<TContext> pipeConfigurator)
        {
            var handlerType = ExtractHandlerType<TConsumer>();
            if (_handlerTransactionOptions.TryGetValue(handlerType, out var transactionOptions))
            {
                AddTransactionBehavior(pipeConfigurator, transactionOptions);
            }
        }

        private static Type ExtractHandlerType<TConsumer>()
            where TConsumer : class
        {
            var validConsumerTypes = new[]
            {
                typeof(QueryHandlerConsumerAdapter<,,>),
                typeof(CommandHandlerConsumerAdapter<,,>),
                typeof(CommandHandlerConsumerAdapter<,>),
                typeof(EventHandlerConsumerAdapter<,>)
            };

            if (!typeof(TConsumer).IsGenericType || !validConsumerTypes.Contains(typeof(TConsumer).GetGenericTypeDefinition()))
            {
                throw new InvalidOperationException();
            }

            return typeof(TConsumer).GetGenericArguments().First();
        }

        private void OnMessageHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e)
        {
            if (_handlerTransactionOptions.ContainsKey(e.MessageHandlerType))
            {
                return;
            }

            var handlerTransactionOptions = new HandlerTransactionOptions
            {
                IsolationLevel = _defaultIsolationLevel,
                Timeout = _defaultTransactionTimeout == default ? TimeSpan.FromSeconds(30) : _defaultTransactionTimeout,
                Suppress = false
            };

            if (e.MessageHandlerType.GetCustomAttributes<SuppressTransactionAttribute>().Any())
            {
                handlerTransactionOptions.Suppress = true;
            }

            var transactionIsolationLevelAttribute = e.MessageHandlerType.GetCustomAttribute<TransactionIsolationLevelAttribute>();
            if (transactionIsolationLevelAttribute != null)
            {
                handlerTransactionOptions.IsolationLevel = transactionIsolationLevelAttribute.IsolationLevel;
                handlerTransactionOptions.Timeout = transactionIsolationLevelAttribute.Timeout;
            }

            _handlerTransactionOptions.Add(e.MessageHandlerType, handlerTransactionOptions);
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) =>
            e.Configurator.ConnectConsumerConfigurationObserver(this);

        private static void AddTransactionBehavior<TContext>(IPipeConfigurator<TContext> configurator, HandlerTransactionOptions options)
            where TContext : class, PipeContext
        {
            if (options.Suppress)
            {
                return;
            }

            configurator.UseTransaction(c =>
            {
                c.Timeout = options.Timeout;
                c.IsolationLevel = options.IsolationLevel;
            });
        }
    }
}
