using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.IoC.Factories;

namespace DotRas.Internal.IoC
{
    partial class ContainerBuilder
    {
        private static void RegisterFactories(Container container)
        {
            RegisterDeviceFactories(container);

            container.Register<ITaskCancellationSourceFactory>(typeof(TaskCancellationSourceFactory));
            container.Register<ITaskCompletionSourceFactory>(typeof(TaskCompletionSourceFactory));
            container.Register<IDeviceTypeFactory>(typeof(DeviceTypeFactory));
            container.Register<IStructFactory>(typeof(StructFactory));
            container.Register<IStructArrayFactory>(typeof(StructFactory));
            container.Register<IRegisteredCallbackFactory>(typeof(RegisteredCallbackFactory));
        }        
    }
}