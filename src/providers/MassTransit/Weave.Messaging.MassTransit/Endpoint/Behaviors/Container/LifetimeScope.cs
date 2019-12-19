namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    public enum LifetimeScope
    {
        /// <summary>
        /// 
        /// </summary>
        Transient = 0,

        /// <summary>
        /// 
        /// </summary>
        Dependency,

        /// <summary>
        /// 
        /// </summary>
        ParentScope,

        /// <summary>
        /// 
        /// </summary>
        Shared
    }
}
