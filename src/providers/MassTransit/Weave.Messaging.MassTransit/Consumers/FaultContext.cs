using System;
using System.Threading;

namespace Weave.Messaging.MassTransit.Consumers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FaultContext
    {
        internal FaultContext()
        {
        }

        private static readonly AsyncLocal<FaultContext> ContextFactory = new AsyncLocal<FaultContext>();

        /// <summary>
        /// 
        /// </summary>
        public static FaultContext Context
        {
            get => ContextFactory.Value;
            set => ContextFactory.Value = value;
        }

        public Guid FaultId { get; internal set; }
        
        public DateTime Timestamp { get; internal set; }
    }
}
