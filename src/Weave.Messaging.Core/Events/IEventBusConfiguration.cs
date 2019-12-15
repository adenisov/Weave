namespace Weave.Messaging.Core.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventBusConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
#pragma warning disable 618
        void RegisterEventHandler<THandler>() where THandler : IEventHandler;
#pragma warning restore 618
    }
}
