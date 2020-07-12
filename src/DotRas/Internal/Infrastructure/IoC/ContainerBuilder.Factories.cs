using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Infrastructure.Factories;

namespace DotRas.Internal.Infrastructure.IoC
{
    partial class ContainerBuilder
    {
        private static void RegisterFactories(Container container)
        {
            RegisterDeviceFactories(container);

            container.Register<IDeviceTypeFactory>(typeof(DeviceTypeFactory));
            container.Register<IStructFactory>(typeof(StructFactory));
            container.Register<IStructArrayFactory>(typeof(StructFactory));
            container.Register<IRegisteredCallbackFactory>(typeof(RegisteredCallbackFactory));
        }        
    }
}