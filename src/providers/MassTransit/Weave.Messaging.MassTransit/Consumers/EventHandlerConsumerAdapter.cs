using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Events;

namespace Weave.Messaging.MassTransit.Consumers
{
    public sealed class EventHandlerConsumerAdapter<THandler, TEvent> : UnresponsiveConsumerBase<TEvent>
        where THandler : IEventHandler<TEvent>
        where TEvent : class, IEventMessage<TEvent>
    {
        private readonly THandler _eventHandler;

        public EventHandlerConsumerAdapter(THandler eventHandler, IEnumerable<IBehavior<TEvent, VoidResponse>> behaviors)
            : base(behaviors)
        {
            _eventHandler = eventHandler;
        }

        protected override async Task HandleVoidResponseAsync(TEvent request, CancellationToken ct)
        {
            await _eventHandler.HandleAsync(request, ct).DropContext();
        }
    }
}
