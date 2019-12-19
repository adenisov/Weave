namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInstanceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        void Register<T>(T instance) where T : class;
    }
}
