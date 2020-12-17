using GreenPipes;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public static class ServiceFactoryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IServiceFactory GetServiceFactory(this PipeContext context) => context.GetPayload<IServiceFactory>();
    }
}
