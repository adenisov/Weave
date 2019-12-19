using System.Threading.Tasks;
using MassTransit;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Events;

namespace Weave.Messaging.MassTransit
{
    public sealed class EventHandlerConsumerAdapter<THandler, TEvent> : IConsumer<TEvent>
        where THandler : IEventHandler<TEvent>
        where TEvent : class, IEventMessage<TEvent>
    {
        private readonly THandler _eventHandler;

        public EventHandlerConsumerAdapter(THandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            await _eventHandler.HandleAsync(context.Message, context.CancellationToken).DropContext();
        }
    }
}