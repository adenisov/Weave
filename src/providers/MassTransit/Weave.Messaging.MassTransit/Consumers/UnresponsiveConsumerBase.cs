using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weave.Messaging.Core;
using Weave.Messaging.MassTransit.Consumers.Behaviors;

namespace Weave.Messaging.MassTransit.Consumers
{
    public abstract class UnresponsiveConsumerBase<TRequest> : ConsumerBase<TRequest, VoidResponse>
        where TRequest : class
    {
        protected UnresponsiveConsumerBase(IEnumerable<IBehavior<TRequest, VoidResponse>> behaviors)
            : base(behaviors)
        {
        }

        protected abstract Task HandleVoidResponseAsync(TRequest request, CancellationToken ct);

        protected sealed override async Task<VoidResponse> HandleInternalAsync(TRequest request, CancellationToken ct)
        {
            await HandleVoidResponseAsync(request, ct).DropContext();
            return VoidResponse.Value;
        }
    }
}
