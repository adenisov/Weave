using System;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;

namespace Weave.Messaging.MassTransit
{
    public static class MessageBusExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="message"></param>
        /// <param name="onConfiguring"></param>
        /// <param name="ct"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task Send<T>(
            this ISendEndpointProvider provider, T message, Action<SendContext<T>> onConfiguring, CancellationToken ct = default)
            where T : class => provider.Send(message, Pipe.Execute(onConfiguring), ct);
    }
}
