using System;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInstanceRegistrationSource
    {
        /// <summary>
        /// 
        /// </summary>
        Type[] AsTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registar"></param>
        void Register(IInstanceRegistration registar);
    }
}