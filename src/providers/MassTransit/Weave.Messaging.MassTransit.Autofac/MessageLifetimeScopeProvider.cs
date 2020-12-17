using Autofac;
using GreenPipes;
using MassTransit;
using MassTransit.AutofacIntegration;

namespace Weave.Messaging.MassTransit.Autofac
{
    internal sealed class MessageLifetimeScopeProvider : ILifetimeScopeProvider
    {
        public MessageLifetimeScopeProvider(IComponentContext container)
        {
            LifetimeScope = container.Resolve<ILifetimeScope>();
        }

        public MessageLifetimeScopeProvider(ILifetimeScope lifetimeScope)
        {
            LifetimeScope = lifetimeScope;
        }

        public ILifetimeScope LifetimeScope { get; }

        public ILifetimeScope GetLifetimeScope<T>(SendContext<T> context) where T : class => ExtractLifetimeScope(context);

        public ILifetimeScope GetLifetimeScope<T>(PublishContext<T> context) where T : class => ExtractLifetimeScope(context);

        public ILifetimeScope GetLifetimeScope(ConsumeContext context) => ExtractLifetimeScope(context);

        public ILifetimeScope GetLifetimeScope<T>(ConsumeContext<T> context) where T : class => ExtractLifetimeScope(context);

        private ILifetimeScope ExtractLifetimeScope(PipeContext context) =>
            context.TryGetPayload<ILifetimeScopeContext>(out var lifetimeScopeContext)
                ? lifetimeScopeContext.LifetimeScope
                : LifetimeScope.BeginLifetimeScope();
    }
}
