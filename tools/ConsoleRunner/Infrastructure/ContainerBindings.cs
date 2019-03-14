using Autofac;
using ConsoleRunner.Infrastructure.Diagnostics;

namespace ConsoleRunner.Infrastructure
{
    class ContainerBindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DotRasLoggingAdapter>().AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}