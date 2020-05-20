using System;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MassTransitEndpointFactory
    {
        private MassTransitEndpointFactory()
        {
        }

        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<MassTransitEndpointFactory> _instanceFactory =
            new Lazy<MassTransitEndpointFactory>(() => new MassTransitEndpointFactory());

        public static MassTransitEndpointFactory Instance => _instanceFactory.Value;
    }
}
