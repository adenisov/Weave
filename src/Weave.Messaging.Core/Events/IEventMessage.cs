namespace Weave.Messaging.Core.Events
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventMessage<TEvent> : IAsyncMessage
        where TEvent : IEventMessage<TEvent>
    {
    }
}
