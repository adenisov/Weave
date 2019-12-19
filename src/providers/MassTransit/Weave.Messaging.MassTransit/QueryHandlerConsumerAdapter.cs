using System.Threading.Tasks;
using MassTransit;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Queries;

namespace Weave.Messaging.MassTransit
{
    public sealed class QueryHandlerConsumerAdapter<THandler, TRequest, TResponse> : IConsumer<TRequest>
        where THandler : IQueryHandler<TRequest, TResponse>
        where TRequest : class, IQueryMessage<TRequest, TResponse>
        where TResponse : class
    {
        private readonly THandler _queryHandler;

        public QueryHandlerConsumerAdapter(THandler queryHandler)
        {
            _queryHandler = queryHandler;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            var response = await _queryHandler.HandleAsync(context.Message, context.CancellationToken).DropContext();
            await context.RespondAsync(response).DropContext();
        }
    }
}
