using System;
using System.Collections.Generic;
using System.Linq;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public sealed class RegistrationBuilder
    {
        private readonly Type _type;
        private readonly ICollection<Type> _as = new HashSet<Type>();

        private LifetimeScope _scope;

        private RegistrationBuilder(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public Type Type => _type;

        /// <summary>
        /// 
        /// </summary>
        public Type[] Registrations => _as.ToArray();

        /// <summary>
        /// 
        /// </summary>
        public LifetimeScope LifetimeScope => _scope;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public RegistrationBuilder As<T>() => As(typeof(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public RegistrationBuilder As(Type t)
        {
            _as.Add(t);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RegistrationBuilder AsSelf()
        {
            _as.Add(_type);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RegistrationBuilder Singleton()
        {
            _scope = LifetimeScope.Shared;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RegistrationBuilder Transient()
        {
            _scope = LifetimeScope.Transient;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RegistrationBuilder PerDependency()
        {
            _scope = LifetimeScope.Dependency;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RegistrationBuilder PerNestedLifetimeScope()
        {
            _scope = LifetimeScope.ParentScope;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RegistrationBuilder RegisterType<T>() => new RegistrationBuilder(typeof(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static RegistrationBuilder RegisterType(Type t) => new RegistrationBuilder(t);
    }
}
