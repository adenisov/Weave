using NLog;
using Weave.Messaging.MassTransit.NLog.Behaviors;

namespace Weave.Messaging.MassTransit.NLog
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        public static MassTransitEndpointBuilder<TBuilder> UseNLog<TBuilder>(this MassTransitEndpointBuilder<TBuilder> builder)
            where TBuilder : MassTransitEndpointBuilder<TBuilder>
        {
            return builder.WithCustomExtension(new NLogEndpointExtension());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="logFactory"></param>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        public static MassTransitEndpointBuilder<TBuilder> UseNLog<TBuilder>(
            this MassTransitEndpointBuilder<TBuilder> builder, LogFactory logFactory)
            where TBuilder : MassTransitEndpointBuilder<TBuilder>
        {
            return builder.WithCustomExtension(new NLogEndpointExtension(logFactory));
        }
    }
}
