using System;
using System.Collections.Generic;
using System.Linq;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public sealed class InstanceRegistrationBuilder<T> : IInstanceRegistrationSource
        where T : class
    {
        private readonly ICollection<Type> _as = new HashSet<Type>();

        private InstanceRegistrationBuilder(T instance)
        {
            Instance = instance;
            _as.Add(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        public T Instance { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type[] AsTypes => _as.ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public InstanceRegistrationBuilder<T> As<TType>() => As(typeof(TType));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public InstanceRegistrationBuilder<T> As(Type t)
        {
            _as.Add(t);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registar"></param>
        public void Register(IInstanceRegistration registar) => registar.Register(Instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static InstanceRegistrationBuilder<T> FromInstance(T instance) => new InstanceRegistrationBuilder<T>(instance);
    }
}
