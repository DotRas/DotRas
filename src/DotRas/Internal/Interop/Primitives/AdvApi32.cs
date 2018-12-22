namespace DotRas.Internal.Interop.Primitives
{
    internal class AdvApi32 : IAdvApi32
    {
        public bool AllocateLocallyUniqueId(out Luid luid)
        {
            return SafeNativeMethods.AllocateLocallyUniqueId(out luid);
        }
    }
}