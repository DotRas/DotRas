using System.Reflection;
using Autofac;
using ConsoleRunner.Infrastructure.Providers;
using DotRas;

namespace ConsoleRunner
{
    partial class Program
    {
        private static IContainer container;

        private static void ConfigureIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            container = builder.Build();

            DependencyResolver.SetLocator(() => new AutofacServiceProvider(container));
        }
    }
}