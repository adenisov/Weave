using System;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    public sealed class InputSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public bool AutoDelete { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool Durable { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool BindMessageExchanges { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? Ttl { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int PrefetchCount { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool PurgeOnStartup { get; set; }
    }
}