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
    /// <typeparam name="TData"></typeparam>
#pragma warning disable 618
    public interface ISaga<TData> : ISaga
#pragma warning restore 618
        where TData : class
    {
    }
}
