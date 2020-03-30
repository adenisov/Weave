using Weave.Messaging.MassTransit.NLog.Behaviors;

namespace Weave.Messaging.MassTransit.NLog
{
    public static class EndpointExtensions
    {
        public static MassTransitEndpointBuilder<TBuilder> UseNLog<TBuilder>(this MassTransitEndpointBuilder<TBuilder> builder)
            where TBuilder : MassTransitEndpointBuilder<TBuilder>
        {
            return builder.WithCustomExtension(new NLogEndpointExtension());
        }
    }
}
