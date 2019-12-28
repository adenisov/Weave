namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContainerRegistar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registar"></param>
        void Register(IContainerRegistration registar);
    }
}
