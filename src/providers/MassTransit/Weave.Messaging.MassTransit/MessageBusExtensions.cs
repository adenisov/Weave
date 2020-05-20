using System;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;

namespace Weave.Messaging.MassTransit
{
    internal static class MessageBusExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busControl"></param>
        /// <param name="message"></param>
        /// <param name="onConfiguring"></param>
        /// <param name="ct"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task Send<T>(
            this IBusControl busControl, T message, Action<SendContext<T>> onConfiguring, CancellationToken ct = default)
            where T : class
        {
            // ToDo: more reliable implementation
            var sendEndpoint = await busControl.GetSendEndpoint(
                    new Uri("rabbitmq://localhost/" + MessageUrn.ForType(typeof(T))))
                .ConfigureAwait(false);

            await sendEndpoint.Send(message, Pipe.Execute(onConfiguring), ct).ConfigureAwait(false);
        }
    }
}
