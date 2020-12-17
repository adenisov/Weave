using System;
using System.Linq;
using Autofac;
using Autofac.Builder;

namespace Weave.Messaging.MassTransit.Autofac
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registration"></param>
        /// <param name="lifetimeScopeTags"></param>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TStyle"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InstancePerMessage<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration,
            params object[] lifetimeScopeTags)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            var tags = new[] { "message" }.Concat(lifetimeScopeTags).ToArray();
            return registration.InstancePerMatchingLifetimeScope(tags);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registration"></param>
        /// <param name="lifetimeScopeTags"></param>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TStyle"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InstancePerPipe<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration,
            params object[] lifetimeScopeTags)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            var tags = new[] { AutofacContainerConfigurator.ScopeName }.Concat(lifetimeScopeTags).ToArray();
            return registration.InstancePerMatchingLifetimeScope(tags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="containerBuilder"></param>
        /// <param name="additionalModules"></param>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        public static MassTransitEndpointBuilder<TBuilder> UseAutofac<TBuilder>(this MassTransitEndpointBuilder<TBuilder> builder,
            ContainerBuilder containerBuilder,
            params Module[] additionalModules)
            where TBuilder : MassTransitEndpointBuilder<TBuilder>
        {
            return builder.WithContainerConfigurator(new AutofacContainerConfigurator(containerBuilder, additionalModules));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="containerBuilderEmitter"></param>
        /// <param name="additionalModules"></param>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        public static MassTransitEndpointBuilder<TBuilder> UseAutofac<TBuilder>(this MassTransitEndpointBuilder<TBuilder> builder,
            Action<ContainerBuilder> containerBuilderEmitter,
            params Module[] additionalModules)
            where TBuilder : MassTransitEndpointBuilder<TBuilder>
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilderEmitter(containerBuilder);
            return builder.WithContainerConfigurator(new AutofacContainerConfigurator(containerBuilder, additionalModules));
        }
    }
}
