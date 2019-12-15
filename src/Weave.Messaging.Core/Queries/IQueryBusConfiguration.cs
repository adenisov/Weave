namespace Weave.Messaging.Core.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueryBusConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
#pragma warning disable 618
        void RegisterQueryHandler<THandler>() where THandler : IQueryHandler;
#pragma warning restore 618
    }
}
