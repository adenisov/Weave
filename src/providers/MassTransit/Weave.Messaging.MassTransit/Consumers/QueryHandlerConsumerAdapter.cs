using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weave.Messaging.Core.Queries;
using Weave.Messaging.MassTransit.Consumers.Behaviors;

namespace Weave.Messaging.MassTransit.Consumers
{
    public sealed class QueryHandlerConsumerAdapter<THandler, TRequest, TResponse> : ConsumerBase<TRequest, TResponse>
        where THandler : IQueryHandler<TRequest, TResponse>
        where TRequest : class, IQueryMessage<TRequest, TResponse>
        where TResponse : class
    {
        private readonly THandler _queryHandler;

        public QueryHandlerConsumerAdapter(THandler queryHandler, IEnumerable<IBehavior<TRequest, TResponse>> behaviors)
            : base(behaviors)
        {
            _queryHandler = queryHandler;
        }

        protected override async Task<TResponse> HandleInternalAsync(TRequest request, CancellationToken ct)
        {
            return await _queryHandler.HandleAsync(request, ct).ConfigureAwait(false);
        }
    }
}
