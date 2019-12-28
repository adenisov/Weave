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
            where TModule : IMessagingModule, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postConfigurationAction"></param>
        void Configure(Action<IBusFactoryConfigurator> postConfigurationAction = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IMassTransitMessageBus CreateMessageBus();
    }
}
