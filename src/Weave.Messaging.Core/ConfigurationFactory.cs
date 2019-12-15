using System;
using JetBrains.Annotations;

namespace Weave.Messaging.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationFactory
    {
        /// <summary>
        /// Compiles a final <typeparamref name="TConfig"/> by a specific compilation delegate <paramref name="configure"/> 
        /// </summary>
        /// <param name="configure">Configuration compilation delegate</param>
        /// <typeparam name="TConfig">Configuration type</typeparam>
        /// <returns>An instance to a compiled configuration of <typeparamref name="TConfig"/></returns>
        [NotNull]
        public static TConfig Configure<TConfig>([CanBeNull] Action<TConfig> configure)
            where TConfig : class, new()
        {
            var config = new TConfig();
            {
                configure?.Invoke(config);
            }

            return config;
        }
    }
}
