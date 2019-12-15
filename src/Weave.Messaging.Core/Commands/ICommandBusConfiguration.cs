namespace Weave.Messaging.Core.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandBusConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
#pragma warning disable 618
        void RegisterCommandHandler<THandler>() where THandler : ICommandHandler;
#pragma warning restore 618

    }
}
