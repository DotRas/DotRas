using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDialParamsBuilder
    {
        RASDIALPARAMS Build(RasDialContext context);
    }
}