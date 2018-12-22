using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDialExtensionsBuilder
    {
        RASDIALEXTENSIONS Build(RasDialContext context);
    }
}