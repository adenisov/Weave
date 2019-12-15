namespace Weave.Messaging.Core.Sagas
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISagaConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSaga"></typeparam>
#pragma warning disable 618
        void RegisterSaga<TSaga>() where TSaga : ISaga;
#pragma warning restore 618
    }
}
