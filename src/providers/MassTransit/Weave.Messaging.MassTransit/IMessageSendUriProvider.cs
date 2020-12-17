using System;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageSendUriProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        Uri GetSendUri<TMessage>();
    }
}
