using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    public abstract class DistinctiveConsumerConfigurationObserver : IConsumerConfigurationObserver
    {
        private readonly ISet<Type> _configuredConsumerTypes = new HashSet<Type>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeConfigurator"></param>
        /// <typeparam name="TConsumer"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        protected abstract void OnConsumerConfigured<TConsumer, TContext>(IPipeConfigurator<TContext> pipeConfigurator)
            where TConsumer : class
            where TContext : class, PipeContext;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator)
            where TConsumer : class
        {
            if (_configuredConsumerTypes.Contains(typeof(TConsumer)))
            {
                return;
            }

            OnConsumerConfigured<TConsumer, ConsumerConsumeContext<TConsumer>>(configurator);
            _configuredConsumerTypes.Add(typeof(TConsumer));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
            where TConsumer : class
            where TMessage : class
        {
            if (_configuredConsumerTypes.Contains(typeof(TConsumer)))
            {
                return;
            }

            OnConsumerConfigured<TConsumer, ConsumerConsumeContext<TConsumer, TMessage>>(configurator);
            _configuredConsumerTypes.Add(typeof(TConsumer));
        }
    }
}
