using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Weave.Messaging.Core;

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

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            var request = context.Message;
            var incomingMessage = new IncomingMessage<TRequest>(request, null);

            Task<TResponse> Handler() => HandleInternalAsync(request, context.CancellationToken);

            var response = await _behaviors
                .Reverse()
                .Aggregate((Func<Task<TResponse>>) Handler, (next, pipeline) => () => pipeline.HandleAsync(incomingMessage, next))()
                .DropContext();

            await context.RespondAsync(response).DropContext();
        }

        public Task Consume(ConsumeContext<Fault<TRequest>> context)
        {
            throw new NotImplementedException();
        }
    }
}
