using System;
using System.Collections.Generic;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Autofac;

namespace Weave.Messaging.MassTransit.Autofac
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class AutofacServiceFactory : IServiceFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacServiceFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public T GetService<T>(string name = "")
        {
            return string.IsNullOrEmpty(name) ? _lifetimeScope.Resolve<T>() : _lifetimeScope.ResolveNamed<T>(name);
        }

        public object GetService(Type type) => _lifetimeScope.Resolve(type);

        public IEnumerable<T> GetServices<T>() => _lifetimeScope.Resolve<IEnumerable<T>>();

        public void Dispose() => _lifetimeScope?.Dispose();
    }
}
