using System;
using Autofac;

namespace ConsoleRunner.Infrastructure.Providers
{
    class AutofacServiceProvider : IServiceProvider
    {
        private readonly IComponentContext context;

        public AutofacServiceProvider(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public object GetService(Type serviceType)
        {
            return context.Resolve(serviceType);
        }
    }
}