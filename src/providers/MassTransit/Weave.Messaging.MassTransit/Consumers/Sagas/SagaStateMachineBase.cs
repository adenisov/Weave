using System;
using Automatonymous;
using Weave.Messaging.Core.Sagas;

namespace Weave.Messaging.MassTransit.Consumers.Sagas
{
    public abstract class SagaStateMachineData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
    }

    public abstract class SagaStateMachineBase<TData> : MassTransitStateMachine<TData>, ISaga<TData>
        where TData : SagaStateMachineData, new()
    {
        public void MarkCompleted() => SetCompletedWhenFinalized();
    }
}
