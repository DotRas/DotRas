using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Services.Dialing
{
    internal class RasDialExtensionsOptionsBuilder
    {
        public RDEOPT Result { get; private set; }

        public void AppendFlagIfTrue(bool check, RDEOPT value)
        {
            if (check)
            {
                Result |= value;
            }
        }
    }
}