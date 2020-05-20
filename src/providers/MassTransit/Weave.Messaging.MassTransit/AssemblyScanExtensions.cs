using System;
using System.Linq;
using System.Reflection;
using Weave.Messaging.Core;

namespace Weave.Messaging.MassTransit
{
    public static class AssemblyScanExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="assemblies"></param>
        public static void RegisterAssemblyMessagingModules(this IMassTransitEndpoint endpoint, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            var resolvedModuleTypes = assemblies
                .SelectMany(_ => _.GetTypes())
                .Where(_ => typeof(IMessagingModule).IsAssignableFrom(_) && !_.IsInterface && !_.IsAbstract);

            foreach (var moduleType in resolvedModuleTypes)
            {
                endpoint.RegisterMessagingModule((IMessagingModule) Activator.CreateInstance(moduleType));
            }
        }
    }
}
