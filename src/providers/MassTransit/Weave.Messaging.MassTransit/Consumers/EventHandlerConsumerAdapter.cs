using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Events;
using Weave.Messaging.MassTransit.Consumers.Behaviors;

namespace Weave.Messaging.MassTransit.Consumers
{
    public sealed class EventHandlerConsumerAdapter<THandler, TRequest> : UnresponsiveConsumerBase<TRequest>
        where THandler : IEventHandler<TRequest>
        where TRequest : class, IEventMessage<TRequest>
    {
        private readonly THandler _eventHandler;

        public EventHandlerConsumerAdapter(THandler eventHandler, IEnumerable<IBehavior<TRequest, VoidResponse>> behaviors)
            : base(behaviors)
        {
            _eventHandler = eventHandler;
        }

        protected override async Task HandleVoidResponseAsync(TRequest request, CancellationToken ct)
        {
            await _eventHandler.HandleAsync(request, ct).DropContext();
        }
    }
}
