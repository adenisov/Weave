using Autofac;

namespace Weave.Messaging.MassTransit.Autofac
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILifetimeScopeContext
    {
        /// <summary>
        /// 
        /// </summary>
        ILifetimeScope LifetimeScope { get; }
    }
}
