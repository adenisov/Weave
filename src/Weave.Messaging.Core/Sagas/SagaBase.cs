using System;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;

namespace Weave.Messaging.Core.Sagas
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class SagaBase<TState> : ISaga<TState>
        where TState : class, new()
    {
        protected SagaBase(TState state)
        {
            State = state ?? new TState();
        }

        protected TState State { get; private set; }

        protected bool IsCompleted { get; private set; }

        public IQueryBus QueryBus { get; set; }

        public ICommandBus CommandBus { get; set; }

        public IEventBus EventBus { get; set; }

        public void MarkCompleted()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("saga is already in completed state.");
            }

            IsCompleted = true;
        }
    }
}
