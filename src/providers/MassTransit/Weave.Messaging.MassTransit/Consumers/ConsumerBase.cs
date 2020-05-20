using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Weave.Messaging.Core;
using Weave.Messaging.MassTransit.Consumers.Behaviors;
using MessageHeaders = Weave.Messaging.Core.MessageHeaders;

namespace Weave.Messaging.MassTransit.Consumers
{
    public abstract class ConsumerBase<TRequest, TResponse> : IConsumer<TRequest>, IConsumer<Fault<TRequest>>
        where TRequest : class
        where TResponse : class
    {
        private readonly IEnumerable<IBehavior<TRequest, TResponse>> _behaviors;

        protected ConsumerBase(IEnumerable<IBehavior<TRequest, TResponse>> behaviors)
        {
            _behaviors = behaviors;
        }

        protected abstract Task<TResponse> HandleInternalAsync(TRequest request, CancellationToken ct);

        public Task Consume(ConsumeContext<TRequest> context) => ConsumeInternal(context, context.Message);

        public async Task Consume(ConsumeContext<Fault<TRequest>> context)
        {
            try
            {
                FaultContext.Context = AssembleFaultContext(context.Message);

                await ConsumeInternal(context, context.Message.Message).ConfigureAwait(false);
            }
            finally
            {
                FaultContext.Context = null;
            }
        }

        private async Task ConsumeInternal(ConsumeContext context, TRequest message)
        {
            var headers = new MessageHeaders();
            ExtractHeaders(headers, context);

            var incomingMessage = new IncomingMessage<TRequest>(headers, message);

            Task<TResponse> Handler() => HandleInternalAsync(incomingMessage.Body, context.CancellationToken);

            var response = await _behaviors
                .Reverse()
                .Aggregate(
                    (Func<Task<TResponse>>) Handler,
                    (next, pipeline) => () =>
                        pipeline.HandleAsync(incomingMessage, next))()
                .ConfigureAwait(false);

            if (response != VoidResponse.Value)
            {
                await context.RespondAsync(response).ConfigureAwait(false);
            }
        }

        private static void ExtractHeaders(MessageHeaders headers, ConsumeContext context)
        {
            foreach (var (key, value) in context.Headers.GetAll().Union(context.ReceiveContext.TransportHeaders.GetAll()))
            {
                headers.WithHeader(
                    key,
                    value == null ? string.Empty : Convert.ToString(value));
            }
        }

        private static FaultContext AssembleFaultContext(Fault context) =>
            new FaultContext
            {
                FaultId = context.FaultId,
                Timestamp = context.Timestamp
            };
    }
}
