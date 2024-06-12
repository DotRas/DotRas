namespace DotRas.Internal.Interop.Primitives {
    internal class AdvApi32 : IAdvApi32 {
        public bool AllocateLocallyUniqueId(out Luid luid) => SafeNativeMethods.AllocateLocallyUniqueId(out luid);
    }
}
