using DotRas.Internal.Abstractions.Composition;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Composition.Factories;

namespace DotRas.Internal.Composition
{
    internal partial class CompositionBuilder
    {
        private static void RegisterFactories(ICompositionRegistry registry)
        {
            RegisterDeviceFactories(registry);

            registry.RegisterCallback<ITaskCancellationSourceFactory>(
                c => new TaskCancellationSourceFactory());

            registry.RegisterCallback<ITaskCompletionSourceFactory>(
                c =>  new TaskCompletionSourceFactory());

            registry.RegisterCallback<IDeviceTypeFactory>(
                c =>  new DeviceTypeFactory(c));

            registry.RegisterCallback<IStructFactory>(
                c =>  new StructFactory(
                    c.GetRequiredService<IMarshaller>()));

            registry.RegisterCallback<IStructArrayFactory>(
                c =>  new StructFactory(
                    c.GetRequiredService<IMarshaller>()));

            registry.RegisterCallback<IRegisteredCallbackFactory>(
                c => new RegisteredCallbackFactory());
        }        
    }
}