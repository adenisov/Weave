using System.Threading;

namespace Weave.Messaging.Core
{
    /// <summary>
    /// Defines a shared message configuration for any type of bus.
    /// </summary>
    public interface IMessageConfiguration
    {
        /// <summary>
        /// A collection of request/response headers. 
        /// </summary>
        MessageHeaders Headers { get; }

        /// <summary>
        /// Cancellation token.
        /// </summary>
        CancellationToken CancellationToken { get; set; }
    }
}
