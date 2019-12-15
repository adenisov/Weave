using System;

namespace Weave.Messaging.Core.Sagas
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("This is just a marker interface. It is not intended for implementation.")]
    public interface ISaga
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
#pragma warning disable 618
    public interface ISaga<TState> : ISaga
#pragma warning restore 618
        where TState : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        void MarkCompleted();
    }
}
