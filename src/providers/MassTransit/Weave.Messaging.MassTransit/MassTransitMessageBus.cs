using System;
using System.Collections.Generic;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;

namespace Weave.Messaging.MassTransit
{
    public sealed class MassTransitMessageBus : IMassTransitMessageBus
    {
        private readonly IBusControl _bus;
        private readonly Lazy<IMessageSendUriProvider> _messageSendUriProvider;
        private readonly IEnumerable<IMessageBusBehavior> _busBehaviors;

        public MassTransitMessageBus(
            IBusControl bus,
            IEnumerable<IMessageBusBehavior> busBehaviors,
            Lazy<IMessageSendUriProvider> messageSendUriProvider)
        {
            _bus = bus;
            _busBehaviors = busBehaviors;
            _messageSendUriProvider = messageSendUriProvider;
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(
            IQueryMessage<TRequest, TResponse> query, Action<QueryConfiguration> configuration = null)
            where TRequest : class, IQueryMessage<TRequest, TResponse>
            where TResponse : class
        {
            var config = ConfigurationFactory.Configure(configuration);

            void ConfigurationCallback(SendContext<TRequest> context)
            {
                foreach (var behavior in _busBehaviors)
                {
                    behavior.HandleRequest(query, context, config);
                }
            }

            var response = await _bus
                .Request<TRequest, TResponse>((TRequest) query, config.CancellationToken, callback: ConfigurationCallback)
                .ConfigureAwait(false);

            return response.Message;
        }

        public async Task SendAsync<TRequest>(ICommandMessage<TRequest> command, Action<CommandConfiguration> configuration = null)
            where TRequest : class, ICommandMessage<TRequest>
        {
            var config = ConfigurationFactory.Configure(configuration);

            void ConfigurationCallback(SendContext<TRequest> context)
            {
                foreach (var behavior in _busBehaviors)
                {
                    behavior.HandleSend(command, context, config);
                }
            }

            var sendEndpoint = await _bus.GetSendEndpoint(_messageSendUriProvider.Value.GetSendUri<TRequest>())
                .ConfigureAwait(false);

            await sendEndpoint.Send(
                    (TRequest) command,
                    Pipe.Execute<SendContext<TRequest>>(ConfigurationCallback),
                    config.CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TResponse> SendAsync<TRequest, TResponse>(
            ICommandMessage<TRequest, TResponse> command, Action<CommandConfiguration> configuration = null)
            where TRequest : class, ICommandMessage<TRequest, TResponse>
            where TResponse : class
        {
            var config = ConfigurationFactory.Configure(configuration);

            void ConfigurationCallback(SendContext<TRequest> context)
            {
                foreach (var behavior in _busBehaviors)
                {
                    behavior.HandleSendWithResult(command, context, config);
                }
            }

            var response = await _bus
                .Request<TRequest, TResponse>((TRequest) command, config.CancellationToken, callback: ConfigurationCallback)
                .ConfigureAwait(false);

            return response.Message;
        }

        public async Task Publish<TEvent>(IEventMessage<TEvent> @event, Action<EventConfiguration> configuration = null)
            where TEvent : class, IEventMessage<TEvent>
        {
            var config = ConfigurationFactory.Configure(configuration);

            void ConfigurationCallback(PublishContext<TEvent> context)
            {
                foreach (var behavior in _busBehaviors)
                {
                    behavior.HandlePublish(@event, context, config);
                }
            }

            await _bus.Publish((TEvent) @event, ConfigurationCallback, config.CancellationToken).ConfigureAwait(false);
        }
    }
}
