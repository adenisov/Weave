using System;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInstanceRegistrationSource<out T> : IContainerRegistar 
        where T : class
    {
        T Instance { get; }
        
        /// <summary>
        /// 
        /// </summary>
        Type[] AsTypes { get; }
    }
}