using System;
using MassTransit;
using Weave.Messaging.Core;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMassTransitEndpoint : IMassTransitMessageHandlerConfiguration, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagingModule"></param>
        /// <typeparam name="TModule"></typeparam>
        void RegisterMessagingModule<TModule>(TModule messagingModule)
            where TModule : IMessagingModule;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        void RegisterMessagingModule<TModule>()
            where TModule : IMessagingModule, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preConfigure"></param>
        /// <param name="postConfigure"></param>
        void Configure(Action<IBusFactoryConfigurator> preConfigure = null, Action<IBusFactoryConfigurator> postConfigure = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IMassTransitMessageBus CreateMessageBus(TimeSpan timeout = default);
    }
}
