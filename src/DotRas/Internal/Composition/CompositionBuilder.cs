namespace DotRas.Internal.Composition
{
    internal static partial class CompositionBuilder
    {
        public static CompositionRoot Build()
        {
            var composer = new CompositionRoot();

            RegisterDiagnostics(composer);
            RegisterFactories(composer);
            RegisterInternal(composer);
            RegisterInterop(composer);

            return composer;
        }
    }
}