using System;
using System.Threading;

namespace Weave.Messaging.Core
{
    /// <summary>
    /// Abstract message configuration builder
    /// </summary>
    public abstract class MessageConfiguration : IMessageConfiguration
    {
        public MessageHeaders Headers { get; } = new MessageHeaders();

        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }

    /// <summary>
    /// Abstract message configuration builder
    /// <remarks>A CRTP builder.</remarks>
    /// </summary>
    /// <typeparam name="TConfig">Configuration type</typeparam>
    public abstract class MessageConfiguration<TConfig> : MessageConfiguration
        where TConfig : MessageConfiguration<TConfig>
    {
        /// <summary>
        /// Set a message id.
        /// </summary>
        /// <param name="messageUid">Message id Guid.</param>
        /// <returns>Reference to builder</returns>
        public TConfig WithMessageId(Guid messageUid)
        {
            Headers.WithMessageId(messageUid);
            return (TConfig) this;
        }

        /// <summary>
        /// Sets the cancellation token.
        /// </summary>
        /// <param name="ct">Instance of cancellation token</param>
        /// <returns>Reference to builder</returns>
        public TConfig WithCancellationToken(CancellationToken ct)
        {
            CancellationToken = ct;
            return (TConfig) this;
        }

        /// <summary>
        /// Sets expiration timeout
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>Reference to builder</returns>
        public TConfig CancelAfter(TimeSpan timeout)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);

            return WithCancellationToken(cts.Token);
        }
    }
}
