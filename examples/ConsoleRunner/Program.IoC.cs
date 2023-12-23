using System.Reflection;
using Autofac;

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
        }
    }
}