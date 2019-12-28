using MassTransit.Topology;
using Weave.Messaging.Debug.UseCases;

namespace Weave.Messaging.MassTransit
{
    public sealed class GlobalEntityNameFormatter : IEntityNameFormatter
    {
        private GlobalEntityNameFormatter()
        {
        }

        public string FormatEntityName<T>()
        {
            if (typeof(T) == typeof(TestQuery))
            {
                return typeof(TestQuery).FullName;
            }

            if (typeof(T) == typeof(TestCommand))
            {
                return typeof(TestCommand).FullName;
            }
            
            if (typeof(T) == typeof(TestCommand2))
            {
                return typeof(TestCommand2).FullName;
            }

            if (typeof(T) == typeof(TestEvent))
            {
                return typeof(T).FullName;
            }
            
            return "amq.fanout";
        }

        internal static readonly IEntityNameFormatter Default = new GlobalEntityNameFormatter();
    }
}
