namespace DotRas.Internal.IoC
{
    internal static partial class ContainerBuilder
    {
        public static ServiceLocator Build()
        {
            var composer = new ServiceLocator();

            RegisterDiagnostics(composer);
            RegisterFactories(composer);
            RegisterInternal(composer);
            RegisterInterop(composer);

            return composer;
        }
    }
}