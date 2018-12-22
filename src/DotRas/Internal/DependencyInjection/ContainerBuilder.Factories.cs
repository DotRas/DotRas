using System.ComponentModel.Design;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection.Factories;

namespace DotRas.Internal.DependencyInjection
{
    internal partial class ContainerBuilder
    {
        private static void RegisterFactories(IServiceContainer container)
        {
            RegisterDeviceFactories(container);

            container.AddService(typeof(ITaskCancellationSourceFactory),
                (c, _) => new TaskCancellationSourceFactory());

            container.AddService(typeof(ITaskCompletionSourceFactory),
                (c, _) => new TaskCompletionSourceFactory());

            container.AddService(typeof(IDeviceTypeFactory),
                (c, _) => new DeviceTypeFactory(c));
            
            container.AddService(typeof(IStructFactory),
                (c, _) => new StructFactory(
                    c.GetRequiredService<IMarshaller>()));

            container.AddService(typeof(IStructArrayFactory),
                (c, _) => new StructFactory(
                    c.GetRequiredService<IMarshaller>()));
        }        
    }
}