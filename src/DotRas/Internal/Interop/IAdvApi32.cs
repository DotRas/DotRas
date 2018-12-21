namespace DotRas.Internal.Interop
{
    internal interface IAdvApi32
    {
        bool AllocateLocallyUniqueId(out Luid luid);
    }
}